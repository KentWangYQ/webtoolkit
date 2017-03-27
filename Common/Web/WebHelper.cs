using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Web
{
    public class WebHelper
    {
        public static string SerializeObject(object obj)
        {
            return SerializeObject(obj, null);
        }
        public static string SerializeObject(object obj, ErrorMsgModel errorMsg)
        {
            var result = new
            {
                rows = obj,
                total = (obj is IEnumerable<object>) ? ((IEnumerable<object>)obj).Count() : obj == null ? 0 : 1,
                errorMsg = errorMsg
            };
            return JsonConvert.SerializeObject(result);
        }
        public static string SerializeObject(object obj, int total, ErrorMsgModel errorMsg = null)
        {
            var result = new
            {
                rows = obj,
                total = total,
                errorMsg = errorMsg
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
