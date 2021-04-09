namespace EM.Terminal.PhotoMirror.Uploader
{
    static class Program
    {
        static void Main()
        {
            System.ServiceProcess.ServiceBase.Run(new System.ServiceProcess.ServiceBase[]
            {
                new CaptureContentUploadService()
            });
        }
    }
}
