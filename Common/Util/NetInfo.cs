using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{
    public class NetInfo
    {

        public static string GetUserIPViaCDN()
        {
            return HttpContext.Current == null ? "" : GetUserIPViaCDN(new HttpContextWrapper(HttpContext.Current).Request);
        }
        public static string GetUserIPViaCDN(HttpRequestBase Request)
        {

            try
            {
                //List<string[]> v = new List<string[]>();
                //foreach (var item in Request.ServerVariables.AllKeys)
                //{
                //    TxtUtility.TxtWrite(item + "---" + Request.ServerVariables[item], TxtUtility.LogType.Message, "Test");
                //    //v.Add(new string[] { item, Request.ServerVariables[item] });
                //}
                string userIP = Request.UserHostAddress;
                if (Request.ServerVariables["HTTP_CDN_SRC_IP"] != null)
                {
                    userIP = Request.ServerVariables["HTTP_CDN_SRC_IP"];
                }
                else
                {
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                }
                return userIP;
            }
            catch (Exception ex) { }
            return "";
        }

        public static string GetUserIPViaCDN(HttpRequestMessage Request)
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;
            return GetUserIPViaCDN(request);
        }

        public static string GetHttpInfo(HttpRequestBase Request)
        {

            try
            {
                //List<string[]> v = new List<string[]>();
                //foreach (var item in Request.ServerVariables.AllKeys)
                //{
                //    TxtUtility.TxtWrite(item + "---" + Request.ServerVariables[item], TxtUtility.LogType.Message, "Test");
                //    //v.Add(new string[] { item, Request.ServerVariables[item] });
                //}
                string info = "";
                if (Request.ServerVariables["ALL_RAW"] != null)
                {
                    info = Request.ServerVariables["ALL_RAW"];
                }
                else
                {
                    info = Request.ServerVariables["ALL_HTTP"];
                }
                return info;
            }
            catch (Exception ex) { }
            return "";
        }

        public static bool CheckMobile(string UserAgent)
        {
            UserAgent = UserAgent.ToLower();
            string pattern = @"(iphone)|(android)|(phone)|(mobile)|(windows phone)|(mqqbrowser)|(wap)|(iPod)|(netfront)|(java)|(opera mobi)|(opera mini)|(ucweb)|(windows ce)|(symbian)|(series)|(webos)|(sony)|(blackberry)|(dopod)|(nokia)|(samsung)|(palmsource)|(xda)|(pieplus)|(meizu)|(midp)|(cldc)|(motorola)|(foma)|(docomo)|(up.browser)|(up.link)|(blazer)|(helio)|(hosin)|(huawei)|(novarra)|(coolpad)|(webos)|(techfaith)|(palmsource)|(alcatel)|(amoi)|(ktouch)|(nexian)|(ericsson)|(philips)|(sagem)|(wellcom)|(bunjalloo)|(maui)|(smartphone)|(iemobile)|(spice)|(bird)|(zte-)|(longcos)|(pantech)|(gionee)|(portalmmm)|(jig browser)|(hiptop)|(benq)|(haier)|(^lct)|(320x320)|(240x320)|(176x220)|(w3c)|(acs-)|(alav)|(alca)|(amoi)|(audi)|(avan)|(benq)|(bird)|(blac)|(blaz)|(brew)|(cell)|(cldc)|(cmd-)|(dang)|(doco)|(eric)|(hipt)|(inno)|(ipaq)|(java)|(jigs)|(kddi)|(keji)|(leno)|(lg-c)|(lg-d)|(lg-g)|(lge-)|(maui)|(maxo)|(midp)|(mits)|(mmef)|(mobi)|(mot-)|(moto)|(mwbp)|(nec-)|(newt)|(noki)|(palm)|(pana)|(pant)|(phil)|(play)|(port)|(prox)|(qwap)|(sage)|(sams)|(sany)|(sch-)|(sec-)|(send)|(seri)|(sgh-)|(shar)|(sie-)|(siem)|(smal)|(smar)|(sony)|(sph-)|(symb)|(t-mo)|(teli)|(tim-)|(tsm-)|(upg1)|(upsi)|(vk-v)|(voda)|(wap-)|(wapa)|(wapi)|(wapp)|(wapr)|(webc)|(winw)|(winw)|(xda)|(xda-)|(Googlebot-Mobile)";
            bool isMoblie = false;
            if (UserAgent != null)
            {
                isMoblie = Regex.IsMatch(UserAgent, pattern);
            }
            //if (System.Web.HttpContext.Current.Request.UserHostAddress == "203.100.82.98")
            //{
            //TxtUtility.TxtWrite("isMobile: " + isMoblie.ToString());
            //TxtUtility.TxtWrite("UserAgent: " + UserAgent);
            //}

            return isMoblie;
        }

        public static string GetNetAddress(string ip, int lenght)
        {
            return LongToIp(IpToLong(ip) & ((1 << lenght) - 1) << (32 - lenght));
        }

        private static string LongToIp(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }

        private static long IpToLong(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }

        public static NetInfo Creator
        {
            get { return new NetInfo(); }
        }
    }
}
