using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.OpenAuth
{
    public class OpenAuthClientSinaWeibo : OpenAuthClientBase
    {
        const string BASE_URL = "https://api.weibo.com/";
        const string AUTH_URL = "oauth2/authorize";
        const string TOKEN_URL = "oauth2/access_token";

        protected override string BaseUrl
        {
            get { return BASE_URL; }
        }

        protected override string AuthorizationCodeUrl
        {
            get { return AUTH_URL; }
        }

        protected override string AccessTokenUrl
        {
            get { return TOKEN_URL; }
        }

        protected override string OpenIdUrl
        {
            get { throw new NotImplementedException(); }
        }

        public OpenAuthClientSinaWeibo(string clientId, string clientSecret, string callbackUrl, string accessToken = null, string openId = null)
            : base(clientId, clientSecret, callbackUrl, accessToken)
        {
            this.ClientName = "SinaWeiboOpenPlatform";
            this.OpenId = openId;

            if (!string.IsNullOrEmpty(accessToken))
                this.isAccessTokenSet = true;
        }

        public override string GetAuthorizationUrl()
        {
            var ub = new UriBuilder(new Uri(new Uri(this.BaseUrl), this.AuthorizationCodeUrl));
            ub.Query = string.Format("client_id={0}&response_type=code&redirect_uri={1}&state={2}", this.ClientId, this.CallbackUrl, OpenAuthClientHelper.Creator.GetState());
            return ub.ToString();
        }

        public override void GetAccessTokenByCode(string code, string state)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state) || !OpenAuthClientHelper.Creator.VerifyState(state))
                return;
            var response = this.HttpPost(this.AccessTokenUrl,
                new
                {
                    client_id = this.ClientId,
                    client_secret = this.ClientSecret,
                    grant_type = "authorization_code",
                    code = code,
                    redirect_uri = this.CallbackUrl
                });

            if (response.StatusCode != HttpStatusCode.OK)
                return;

            var result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if (result["access_token"] == null)
                return;

            this.AccessToken = result.Value<string>("access_token");
            this.ExpiresIn = result.Value<int>("expires_in");
            this.OpenId = result.Value<string>("uid");

            this.isAccessTokenSet = true;
        }

        public override void GetOpenId()
        {
            throw new NotImplementedException();
        }

        public override Task<HttpResponseMessage> HttpGetAsync(string api, Dictionary<string, object> parameters = null)
        {
            if (this.IsAuthorized)
            {
                if (parameters == null)
                    parameters = new Dictionary<string, object>();
                if (!parameters.ContainsKey("uid"))
                    parameters["uid"] = this.OpenId;
                if (!parameters.ContainsKey("access_token"))
                    parameters["access_token"] = this.AccessToken;
            }
            return base.HttpGetAsync(api, parameters);
        }

        public override Task<HttpResponseMessage> HttpPostAsync(string api, Dictionary<string, object> parameters)
        {
            if (this.IsAuthorized)
            {
                if (parameters == null)
                    parameters = new Dictionary<string, object>();
                if (!parameters.ContainsKey("uid"))
                    parameters["uid"] = this.OpenId;
                if (!parameters.ContainsKey("access_token"))
                    parameters["access_token"] = this.AccessToken;
            }
            return base.HttpPostAsync(api, parameters);
        }
    }
}
