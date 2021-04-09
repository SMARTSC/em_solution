namespace EM.Terminal.PhotoMirror.Managers
{
    public sealed class ContentUploadServiceManager
    {
        private static ContentUploadServiceManager instance;

        public static ContentUploadServiceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContentUploadServiceManager();
                }

                return instance;
            }
        }

        public void StartService()
        {
            using (var controller = new System.ServiceProcess.ServiceController(Common.Providers.CommonConstantProvider.UPLOAD_SERVICE_NAME))
            {
                if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    controller.Stop();
                    controller.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, System.TimeSpan.FromSeconds(10.0));
                }

                var contentCaptureDirectoryPath = Providers.ConstantProvider.ContentCaptureDirectoryPath;

                if (!System.IO.Directory.Exists(contentCaptureDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(contentCaptureDirectoryPath);
                }

                controller.Start(new string[]
                {
                    SessionManager.Instance.AuthenticationType,
                    SessionManager.Instance.AuthorizationToken,
                    contentCaptureDirectoryPath
                });
            }
        }

        public void StopServiceIfRunning()
        {
            using (var controller = new System.ServiceProcess.ServiceController(Common.Providers.CommonConstantProvider.UPLOAD_SERVICE_NAME))
            {
                if (controller.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    controller.Stop();
                    controller.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, System.TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
