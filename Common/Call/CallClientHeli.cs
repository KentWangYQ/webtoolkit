using Common.Model;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Call
{
    public class CallClientHeli : CallClientBase
    {
        readonly string BASE_URL = ConfigurationManager.AppSettings["CallClientBaseHeli_BaseUrl"];
        readonly string CALL_API = ConfigurationManager.AppSettings["CallClientBaseHeli_CallApi"];

        protected override string BaseUrl
        {
            get { return BASE_URL; }
        }

        protected override string CallApi
        {
            get { return CALL_API; }
        }


        public override string GetCallApiUrl()
        {
            var ub = new UriBuilder(new Uri(new Uri(this.BASE_URL), this.CallApi));
            return ub.ToString();
        }


        public override CallStatusCode Call(string apiUrl, dynamic parameters = null)
        {
            if (string.IsNullOrEmpty(apiUrl))
                return CallStatusCode.Error;

            try
            {
                System.Net.Http.HttpResponseMessage response = new HttpHelper().HttpGet(apiUrl, parameters);

                if (response.StatusCode != HttpStatusCode.OK)
                    return CallStatusCode.CallServiceRequestError;

                var result = response.Content.ReadAsStringAsync().Result;

                switch (result.ToLower())
                {
                    case "y":
                        return CallStatusCode.Success;
                    default:
                        return CallStatusCode.CallServiceReturnError;
                }
            }
            catch (Exception ex)
            {
                return CallStatusCode.CallServiceRequestError;
            }
        }

        public override CallStatusCode Call(string apiUrl, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(apiUrl))
                return CallStatusCode.Error;

            var response = new HttpHelper().HttpGet(apiUrl, parameters);

            if (response.StatusCode != HttpStatusCode.OK)
                return CallStatusCode.Error;

            var result = response.Content.ReadAsStringAsync().Result;

            switch (result.ToLower())
            {
                case "y":
                    return CallStatusCode.Success;
                default:
                    return CallStatusCode.Error;
            }
        }

    }
}
