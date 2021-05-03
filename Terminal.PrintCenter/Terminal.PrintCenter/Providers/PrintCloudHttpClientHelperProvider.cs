namespace EM.Terminal.PrintCenter.Providers
{
    public class PrintCloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private readonly string host;
        private readonly int port;
        private readonly string schema;

        public PrintCloudHttpClientHelperProvider(string host, int port, bool isSecure)
        {
            this.host = host;
            this.port = port;
            
            schema = isSecure ? "https" : "http";
        }

        private System.Collections.Generic.KeyValuePair<string, string>[] GetExtraHeaders()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>(Print.Api.Common.Providers.ConstantProvider.API_KEY_HEADER_PROPERTY, "v0.1.0-alpha-fyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ8")
            };
        }

        #region IHttpClientHelperProviderFacade implementation

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}:{2}/api/", schema, host, port), null, GetExtraHeaders);

        #endregion
    }
}
