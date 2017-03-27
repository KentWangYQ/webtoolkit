using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ConfigConstants
    {

        public static string[] AllowOrgins = ConfigurationManager.AppSettings["cors"].ToString().Split(',');

        static public TimeSpan SmsCodeDuration = TimeSpan.FromSeconds(int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmsCodeDuration"]));
        static public int SmsLimitCycle = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmsLimitCycle"]);

        static public int IpLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IpLimit"]);
        static public int MobileLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MobileLimit"]);

        static public int IpCheckLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["IpLimit"]) * 3;
        static public int MobileCheckLimit = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MobileLimit"]) * 3;

        static public int CaptchaCodeLength = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CaptchaCodeLength"].ToString());
        static public TimeSpan CaptchaDuration = TimeSpan.FromSeconds(int.Parse(System.Configuration.ConfigurationManager.AppSettings["CaptchaTimeOut"].ToString()));


        static public List<string> WhiteListIp = System.Configuration.ConfigurationManager.AppSettings["WhiteList:Ip"].ToString().Split(';').ToList();
        static public List<string> WhiteListMobile = System.Configuration.ConfigurationManager.AppSettings["WhiteList:Mobile"].ToString().Split(';').ToList();

        static public string CaptchaCacheKey = "Captcha_cache_api";
        static public string SmsCacheKey = "Sms_mobile_code_cache";
    }
}
