namespace EM.Terminal.PhotoMirror.Common.Providers
{
    public static class CommonConstantProvider
    {
        public const string UPLOAD_SERVICE_NAME = "PhotoMirrorUploader";

        public const string UPLOAD_SERVICE_DISPLAY_NAME = "PhotoMirror Upload Service";

        public const string UPLOAD_SERVICE_DESCRIPTION = "Servicio de subida de capturas de la aplicación PhotoMirror a servidor cloud de la plataforma.";

#if DEBUG
        public const string CLOUD_CONNECTION_SCHEMA = "http";
        public const string CLOUD_SERVER_HOST = "localhost";
#else
        public const string CLOUD_CONNECTION_SCHEMA = "https";
        public const string CLOUD_SERVER_HOST = "cloud.elespejomolon.com";
#endif
    }
}
