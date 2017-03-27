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
    public class HuYiSMS : ISMS
    {
        string urlTemplate = System.Configuration.ConfigurationManager.AppSettings["huyiSMS"];


        public string Send(string mobile, string content)
        {
            string url = string.Format(urlTemplate, mobile, content);
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.Default;
                return client.DownloadString(url);
            }
        }

        public string Send(string mobile, string content, Encoding encoding)
        {
            string url = string.Format(urlTemplate, mobile, content);
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                return client.DownloadString(url);
            }
        }

        public void SendAsync(string mobile, string content, System.Net.DownloadStringCompletedEventHandler DownloadStringCompleted)
        {
            string url = string.Format(urlTemplate, mobile, content);
            using (WebClient client = new WebClient())
            {
                if (DownloadStringCompleted != null)
                    client.DownloadStringCompleted += DownloadStringCompleted;
                client.DownloadStringAsync(new Uri(url), url);
            }
        }

        public void SendAsync(string mobile, string content, System.Net.DownloadDataCompletedEventHandler DownloadDataCompleted)
        {
            string url = string.Format(urlTemplate, mobile, content);
            using (WebClient client = new WebClient())
            {
                if (DownloadDataCompleted != null)
                    client.DownloadDataCompleted += DownloadDataCompleted;
                client.DownloadDataAsync(new Uri(url), url);
            }
        }


        public VerifyStatusCode StatusCheck(string result)
        {
            var code = "";
            if (result.StartsWith("<?xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                manager.AddNamespace("n", "http://106.ihuyi.cn/");
                code = doc.SelectSingleNode("//n:SubmitResult/n:code", manager).InnerText;
            }
            else
            { code = JsonConvert.DeserializeObject<Result>(result).code; }

            var r = VerifyStatusCode.Error;

            switch (code)
            {
                case "2":
                    r = VerifyStatusCode.Success;
                    break;
                case "4058":
                    r = VerifyStatusCode.SmsOutOfMobileLimit;
                    break;
                default:
                    r = VerifyStatusCode.SmsServiceError;
                    break;
            }
            return r;
        }

        class Result
        {
            public string code { get; set; }
            public string msg { get; set; }
            public string smsid { get; set; }
        }
    }
}
