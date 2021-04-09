namespace EM.Terminal.PhotoMirror.Providers
{
    public sealed class HttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private static HttpClientHelperProvider instance;

        public static HttpClientHelperProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HttpClientHelperProvider();
                }

                return instance;
            }
        }

        private HttpClientHelperProvider() { }

        private System.Collections.Generic.KeyValuePair<string, string> AuthorizeHeader
        {
            get
            {
                if (!Managers.SessionManager.Instance.HasAuthenticatedSession)
                {
                    return default;
                }
                else
                {
                    var type = Managers.SessionManager.Instance.AuthenticationType;
                    var token = Managers.SessionManager.Instance.AuthorizationToken;

                    return new System.Collections.Generic.KeyValuePair<string, string>(type, token);
                }
            }
        }

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", Common.Providers.CommonConstantProvider.CLOUD_CONNECTION_SCHEMA, Common.Providers.CommonConstantProvider.CLOUD_SERVER_HOST), () => AuthorizeHeader, null);
    }
}
