namespace EM.Terminal.PrintCenter.Providers
{
    public class CloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
#if DEBUG
        public const string CLOUD_CONNECTION_SCHEMA = "http";
        public const string CLOUD_SERVER_HOST = "localhost";
#else
        public const string CLOUD_CONNECTION_SCHEMA = "https";
        public const string CLOUD_SERVER_HOST = "cloud.elespejomolon.com";
#endif

        private System.Collections.Generic.KeyValuePair<string, string> AuthorizeHeader
        {
            get
            {
                var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

                if (!sessionManager.HasAuthenticatedSession)
                {
                    return default;
                }
                else
                {
                    var type = sessionManager.AuthenticationType;
                    var token = sessionManager.AuthorizationToken;

                    return new System.Collections.Generic.KeyValuePair<string, string>(type, token);
                }
            }
        }

        #region IHttpClientHelperProviderFacade implementation

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", CLOUD_CONNECTION_SCHEMA, CLOUD_SERVER_HOST), () => AuthorizeHeader, null);

        #endregion
    }
}
