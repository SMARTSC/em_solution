namespace EM.Print.Enqueuer.StanIn.Providers
{
    public sealed class PrintCloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private readonly string authenticationToken;

        public PrintCloudHttpClientHelperProvider(string host, int port, bool isSecureConnectionRequired)
        {
            authenticationToken = System.Configuration.ConfigurationManager.AppSettings["PrintCloudServerAuthenticationToken"];

            HttpClientHelper = new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}:{2}/api/", isSecureConnectionRequired ? "https" : "http", host, port), extraHeadersGetter: GetExtraHeaders);
        }

        private System.Collections.Generic.KeyValuePair<string, string>[] GetExtraHeaders()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>(Api.Common.Providers.ConstantProvider.API_KEY_HEADER_PROPERTY, authenticationToken)
            };
        }

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper { get; private set; }
    }
}
