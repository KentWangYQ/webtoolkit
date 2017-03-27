using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common.Message
{
    public class RongLianSMS : ISMS
    {
        static string url = "", accountSid = "", accountToken = "", appId = "", templateId = "", duration = "";
        string[] config = System.Configuration.ConfigurationManager.AppSettings["ronglianSMS"].ToString().Split(',');

        public RongLianSMS()
        {
            foreach (var item in config)
            {
                var c = item.Split(':');
                if (c.Length >= 2)
                {
                    switch (c[0].ToLower())
                    {
                        case "url":
                            url = c[1];
                            break;
                        case "accountsid":
                            accountSid = c[1];
                            break;
                        case "accounttoken":
                            accountToken = c[1];
                            break;
                        case "appid":
                            appId = c[1];
                            break;
                        case "templateid":
                            templateId = c[1];
                            break;
                        case "duration":
                            duration = c[1];
                            break;
                    }
                }
            }

            url += "?sig=" + Encrypt.MD5(accountSid + accountToken + DateTime.Now.ToString("yyyyMMddHHmmss")).ToUpper();
        }

        public string Send(string mobile, string content)
        {
            var send = new
            {
                to = mobile,
                appId = appId,
                templateId = templateId,
                datas = new string[] { content, duration }
            };
            byte[] sendData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(send));

            string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(accountSid + ":" + DateTime.Now.ToString("yyyyMMddHHmmss")));

            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Accept", "application/json");
                client.Headers.Add("Content-Type", "application/json;charset=utf-8");
                client.Headers.Add("ContentLength", sendData.Length.ToString());
                client.Headers.Add("Authorization", authorization);
                return Encoding.UTF8.GetString(client.UploadData(System.Web.HttpUtility.UrlDecode(url), "POST", sendData));
            }
        }

        public string Send(string mobile, string content, Encoding encoding)
        {
            string sendjson = "{\"to\":\"" + mobile + "\",\"appId\":\"" + appId + "\",\"templateId\":\"" + templateId + "\",\"datas\":[\"" + content + "\"]}";
            byte[] sendData = Encoding.UTF8.GetBytes(sendjson);

            string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(accountSid + ":" + DateTime.Now.ToString("yyyyMMddHHmmss")));

            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                client.Headers.Add("Accept", "application/json");
                client.Headers.Add("Content-Type", "application/json;charset=utf-8");
                client.Headers.Add("Content-Length", sendData.Length.ToString());
                client.Headers.Add("Authorization", authorization);
                return Encoding.UTF8.GetString(client.UploadData(url, sendData));
            }
        }


        public void SendAsync(string mobile, string content, DownloadStringCompletedEventHandler DownloadStringCompleted)
        {
            throw new NotImplementedException();
        }

        public void SendAsync(string mobile, string content, DownloadDataCompletedEventHandler DownloadDataCompleted)
        {
            throw new NotImplementedException();
        }


        public VerifyStatusCode StatusCheck(string result)
        {
            var code = "";
            if (result.StartsWith("<?xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                code = doc.SelectSingleNode("//Response/statusCode").InnerText;
            }
            else
            { code = JsonConvert.DeserializeObject<Result>(result).statusCode; }

            var r = VerifyStatusCode.Error;

            switch (code)
            {
                case "000000":
                    r = VerifyStatusCode.Success;
                    break;
                default:
                    r = VerifyStatusCode.SmsServiceError;
                    break;
            }
            return r;
        }
    }

    class Result
    {
        public string statusCode { get; set; }
        public TemplateSMS TemplateSMS { get; set; }
    }

    class TemplateSMS
    {
        public string dateCreated { get; set; }
        public string smsMessageSid { get; set; }
    }
}
