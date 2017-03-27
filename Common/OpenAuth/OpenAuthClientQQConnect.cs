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
    public class OpenAuthClientQQConnect : OpenAuthClientBase
    {
        const string BASE_URL = "https://graph.qq.com/";
        const string AUTH_URL = "oauth2.0/authorize";
        const string TOKEN_URL = "oauth2.0/token";
        const string OPENID_URL = "oauth2.0/me";
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
            get { return OPENID_URL; }
        }

        public OpenAuthClientQQConnect(string clientId, string clientSecret, string callbackUrl, string accessToken, string openId)
            : base(clientId, clientSecret, callbackUrl, accessToken)
        {
            this.ClientName = "QQConnect";
            this.OpenId = openId;

            if (!string.IsNullOrEmpty(this.AccessToken))
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
            var response = this.HttpPost(this.AccessTokenUrl, new
            {
                grant_type = "authorization_code",
                client_id = this.ClientId,
                client_secret = this.ClientSecret,
                code = code,
                redirect_uri = this.CallbackUrl
            });

            if (response.StatusCode != HttpStatusCode.OK)
                return;

            try
            {
                var result = response.Content.ReadAsStringAsync().Result.Split('&').ToDictionary(k => k.Split('=')[0], v => v.Split('=')[1]);
                if (result["access_token"] == null)
                    return;

                this.AccessToken = result["access_token"];

                var expires = 0;
                int.TryParse(result["expires_in"], out expires);
                this.ExpiresIn = expires;
                this.RefreshToken = result["refresh_token"];

                this.isAccessTokenSet = true;
            }
            catch (Exception ex) { }
        }

        public override void GetOpenId()
        {
            var response = this.HttpGet(this.OpenIdUrl, new
            {
                access_token = this.AccessToken
            });

            if (response.StatusCode != HttpStatusCode.OK)
                return;

            try
            {
                var result = JObject.Parse(string.Join(response.Content.ReadAsStringAsync().Result.Split(new char[] { '{', '}' })[1], "{", "}"));

                this.OpenId = result.Value<string>("openid");
            }
            catch (Exception ex) { }
        }

        public override Task<HttpResponseMessage> HttpGetAsync(string api, Dictionary<string, object> parameters = null)
        {
            if (this.IsAuthorized)
            {
                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                if (!parameters.ContainsKey("access_token"))
                    parameters["access_token"] = this.AccessToken;

                if (!parameters.ContainsKey("oauth_consumer_key"))
                    parameters["oauth_consumer_key"] = this.ClientId;

                if (!parameters.ContainsKey("openid"))
                    parameters["openid"] = this.ClientId;
            }
            return base.HttpGetAsync(api, parameters);
        }

        public override Task<HttpResponseMessage> HttpPostAsync(string api, Dictionary<string, object> parameters)
        {
            if (this.IsAuthorized)
            {
                if (parameters == null)
                    parameters = new Dictionary<string, object>();

                if (!parameters.ContainsKey("access_token"))
                    parameters["access_token"] = this.AccessToken;

                if (!parameters.ContainsKey("oauth_consumer_key"))
                    parameters["oauth_consumer_key"] = this.ClientId;

                if (!parameters.ContainsKey("openid"))
                    parameters["openid"] = this.ClientId;
            }
            return base.HttpPostAsync(api, parameters);
        }
    }
}
