namespace EM.Print.Enqueuer.Providers
{
    public sealed class PrintCloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private readonly string authenticationToken;

        public PrintCloudHttpClientHelperProvider()
        {
            authenticationToken = System.Configuration.ConfigurationManager.AppSettings["PrintCloudServerAuthenticationToken"];

            var host = System.Configuration.ConfigurationManager.AppSettings["PrintCloudServerHost"];
            var port = System.Configuration.ConfigurationManager.AppSettings["PrintCloudServerPort"];
            var schema = System.Configuration.ConfigurationManager.AppSettings["PrintCloudServerSchema"];

            HttpClientHelper = new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}:{2}/api/", schema, host, port), extraHeadersGetter: GetExtraHeaders);
        }

        private System.Collections.Generic.KeyValuePair<string, string>[] GetExtraHeaders()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>(Print.Api.Common.Providers.ConstantProvider.API_KEY_HEADER_PROPERTY, authenticationToken)
            };
        }

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper { get; private set; }
    }
}
