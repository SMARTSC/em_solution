namespace EM.Terminal.Common.Providers
{
    public static class ConstantProvider
    {
//#if DEBUG
//        public const string CLOUD_CONNECTION_SCHEMA = "http";
//        public const string CLOUD_SERVER_HOST = "localhost";
//#else
        public const string CLOUD_CONNECTION_SCHEMA = "https";
        public const string CLOUD_SERVER_HOST = "cloud.elespejomolon.com";
//#endif
    }
}
