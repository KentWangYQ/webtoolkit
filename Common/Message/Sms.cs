using Common;
using Common.Frequency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public class Sms
    {
        /// <summary>
        /// Send Sms
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="ip"></param>
        /// <returns>StatusCode Start with 1 mean success</returns>
        public VerifyStatusCode Send(string mobile, string ip)
        {
            string code = new Random().Next(1000000).ToString().PadLeft(6, '0');
            //string code
            //Limit Check
            var limit = LimitCheck(mobile, ip);
            if (limit != VerifyStatusCode.Success)
            {
                return limit;
            }

            //Code Cache
            CodeCache(mobile, code);

            //Message Send
            ISMS sms1 = new RongLianSMS();
            var result1 = sms1.Send(mobile, code);
            if (sms1.StatusCheck(result1) != VerifyStatusCode.Success)
            {
                ISMS sms2 = new HuYiSMS();
                string result2 = sms2.Send(mobile, code, Encoding.UTF8);
                if (sms2.StatusCheck(result2) != VerifyStatusCode.Success)
                    return VerifyStatusCode.SmsServiceError;
                else
                    return VerifyStatusCode.SmsHuyiSended;
            }
            else
                return VerifyStatusCode.SmsRonglianSended;
            return VerifyStatusCode.Success;
        }

        public VerifyStatusCode LimitCheck(string mobile, string ip)
        {
            //Check ip limit
            if (ConfigConstants.WhiteListIp.IndexOf(ip) < 0 && FrequencyControlSms.Creator.IsOverFrequency(ip, null, DateTime.UtcNow.AddDays(1).Date, ConfigConstants.IpLimit))
                return VerifyStatusCode.SmsOutOfIPLimit;


            //Check mobile limit
            if (ConfigConstants.WhiteListMobile.IndexOf(mobile) < 0 && FrequencyControlSms.Creator.IsOverFrequency(mobile, null, DateTime.UtcNow.AddDays(1).Date, ConfigConstants.MobileLimit))
                return VerifyStatusCode.SmsOutOfMobileLimit;

            return VerifyStatusCode.Success;
        }

        private void CodeCache(string mobile, string code)
        {
            string cacheKey = "Sms_mobile_code_cache";
            CollectionCache.Insert(cacheKey, mobile, new string[] { code, "0", "" }, ConfigConstants.SmsCodeDuration);
        }
    }

    class SmsModel
    {
        public string Mobile { get; set; }
        public string IP { get; set; }
        public string Code { get; set; }
        public int Hit { get; set; }
    }
}
