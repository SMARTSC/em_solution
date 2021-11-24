namespace EM.Print.Enqueuer.StanIn.Providers
{
    public class ContentCloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private readonly string authenticationType;
        private readonly string authorizationToken;

        public ContentCloudHttpClientHelperProvider(string authenticationType, string authorizationToken)
        {
            this.authenticationType = authenticationType;
            this.authorizationToken = authorizationToken;

            HttpClientHelper = new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", ConstantProvider.CLOUD_CONNECTION_SCHEMA, ConstantProvider.CLOUD_SERVER_HOST), GetAuthorization);
        }

        public System.Collections.Generic.KeyValuePair<string, string> GetAuthorization()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>(authenticationType, authorizationToken);
        }

        #region IHttpClientHelperProviderFacade implementation

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper { get; private set; }

        #endregion
    }
}
