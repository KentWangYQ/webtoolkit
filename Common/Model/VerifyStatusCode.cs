using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum VerifyStatusCode
    {
        Error = 0,
        Success = 1,
        SmsRonglianSended = 101,
        SmsHuyiSended = 102,

        //Sms
        SmsServiceError = 301,
        SmsOutOfIPLimit = 302,
        SmsOutOfMobileLimit = 303,
        SmsNotMatch = 304,
        SmsOutOfCheckLimit = 305,
        SmsCodeOutOfTime = 306,

        //Captcha
        CaptchaNotMatch = 401,
        CaptchaOutOfTime = 402,
    }
    public class VerifyStatusMessage
    {
        public static string GetMessage(VerifyStatusCode s)
        {
            string result = "";
            switch (s)
            {
                case VerifyStatusCode.Success:
                    result = "操作成功";
                    break;
                case VerifyStatusCode.Error:
                    result = "操作失败";
                    break;
                case VerifyStatusCode.SmsServiceError:
                    result = "短信服务错误";
                    break;
                case VerifyStatusCode.SmsOutOfIPLimit:
                    result = "IP超出短信操作限制";
                    break;
                case VerifyStatusCode.SmsOutOfMobileLimit:
                    result = "手机号超出短信操作限制";
                    break;
                case VerifyStatusCode.SmsNotMatch:
                    result = "手机验证码错误";
                    break;
                case VerifyStatusCode.SmsOutOfCheckLimit:
                    result = "手机验证码连续三次验证失败，请重新获取手机验证码";
                    break;
                case VerifyStatusCode.SmsCodeOutOfTime:
                    result = "手机验证码已失效，请重新获取手机验证码";
                    break;
                case VerifyStatusCode.CaptchaNotMatch:
                    result = "图形验证码错误";
                    break;
                case VerifyStatusCode.CaptchaOutOfTime:
                    result = "图形验证码失效";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
    }
}
