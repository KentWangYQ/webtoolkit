using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public enum CallStatusCode
    {
        Error = 0,
        Success = 1,

        CallServiceRequestError = 301,
        CallServiceReturnError = 302,
        
        CallOverFrequency=401,
        CallIPOverFrequency = 402,
        CallMobileOverFrequency = 403
    }

    public class CallStatusMessage
    {
        public static string GetMessage(CallStatusCode s)
        {
            string result = "";
            switch (s)
            {
                case CallStatusCode.Success:
                    result = "操作成功";
                    break;
                case CallStatusCode.Error:
                    result = "操作失败";
                    break;
                case CallStatusCode.CallServiceRequestError:
                    result = "呼叫请求错误";
                    break;
                case CallStatusCode.CallServiceReturnError:
                    result = "呼叫服务返回错误";
                    break;
                case CallStatusCode.CallOverFrequency:
                    result = "呼叫频率超出限制";
                    break;
                case CallStatusCode.CallIPOverFrequency:
                    result = "该IP呼叫次数累计超出限制";
                    break;
                case CallStatusCode.CallMobileOverFrequency:
                    result = "该手机号呼叫次数累计超出限制";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
    }
}
