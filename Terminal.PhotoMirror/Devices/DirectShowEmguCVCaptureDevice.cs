namespace EM.Terminal.PhotoMirror.Devices
{
    public class DirectShowEmguCVCaptureDevice : Facade.ICaptureDeviceFacade
    {
        private readonly DirectShowLib.DsDevice directShowDevice;
        private readonly int directShowIndex;

        public DirectShowEmguCVCaptureDevice(DirectShowLib.DsDevice directShowDevice, int directShowIndex)
        {
            this.directShowDevice = directShowDevice;
            this.directShowIndex = directShowIndex;
        }

        public void Capture(System.Guid captureImageId, System.Action successActionCallback, System.Action<System.Exception> failActionCallback)
        {
            try
            {
                using (var capture = new Emgu.CV.VideoCapture(directShowIndex, Emgu.CV.VideoCapture.API.DShow))
                {
                    var frame = capture.QueryFrame();

                    var contentCaptureDirectoryPath = Providers.ConstantProvider.ContentCaptureDirectoryPath;

                    if (!System.IO.Directory.Exists(contentCaptureDirectoryPath))
                    {
                        System.IO.Directory.CreateDirectory(contentCaptureDirectoryPath);
                    }

                    frame.Save(System.IO.Path.Combine(contentCaptureDirectoryPath, string.Format("{0}.JPG", captureImageId)));

                    successActionCallback?.Invoke();
                }
            }
            catch (System.Exception exception)
            {
                failActionCallback?.Invoke(exception);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} (DirectShow + Emgu OpenCV)", directShowDevice.Name);
        }
    }
}
