namespace EM.Terminal.PhotoMirror.Providers
{
    public static class ConstantProvider
    {
        public static string ContentCaptureDirectoryPath => System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Captures");

        public static string CapturedImageBackUpDirectoryPath => System.IO.Path.Combine(ContentCaptureDirectoryPath, "BackUp");
    }
}
