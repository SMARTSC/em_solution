namespace EM.Terminal.PrintCenter.Providers
{
    public class PrintCloudHttpClientHelperProvider : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
#if DEBUG
        public const string CLOUD_CONNECTION_SCHEMA = "http";
        public const string CLOUD_SERVER_HOST = "localhost:5000";
#else
        public const string CLOUD_CONNECTION_SCHEMA = "https";
        public const string CLOUD_SERVER_HOST = "printcloud.elespejomolon.net";
#endif

        private System.Collections.Generic.KeyValuePair<string, string>[] GetExtraHeaders()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>(Print.Api.Common.Providers.ConstantProvider.API_KEY_HEADER_PROPERTY, "v0.1.0-alpha-fyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ8")
            };
        }

        #region IHttpClientHelperProviderFacade implementation

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", CLOUD_CONNECTION_SCHEMA, CLOUD_SERVER_HOST), null, GetExtraHeaders);

        #endregion
    }
}
