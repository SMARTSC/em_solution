namespace EM.Print.Enqueuer.StanIn.Providers
{
    public class AnonymousContentCloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        public AnonymousContentCloudHttpClientHelperProvider()
        {
            HttpClientHelper = new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", ConstantProvider.CLOUD_CONNECTION_SCHEMA, ConstantProvider.CLOUD_SERVER_HOST));
        }

        #region IHttpClientHelperProviderFacade implementation

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper { get; private set; }

        #endregion
    }
}
