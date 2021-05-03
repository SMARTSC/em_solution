namespace EM.Terminal.PrintCenter.Providers
{
    public class CloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
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

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", Common.Providers.ConstantProvider.CLOUD_CONNECTION_SCHEMA, Common.Providers.ConstantProvider.CLOUD_SERVER_HOST), () => AuthorizeHeader, null);

        #endregion
    }
}
