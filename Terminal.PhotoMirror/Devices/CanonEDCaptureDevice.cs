namespace EM.Terminal.PhotoMirror.Devices
{
    public class CanonEDCaptureDevice : Facade.ICaptureDeviceFacade
    {
        private readonly Driver.Canon.Camera canonCamera;

        private System.Action successActionCallback;
        private System.Action<System.Exception> failActionCallback;

        private System.Guid targetCaptureImageId;

        public CanonEDCaptureDevice(Driver.Canon.Camera canonCamera)
        {
            this.canonCamera = canonCamera;
        }

        public void Capture(System.Guid captureImageId, System.Action successActionCallback, System.Action<System.Exception> failActionCallback)
        {
            targetCaptureImageId = captureImageId;

            if (canonCamera.SessionOpen)
            {
                canonCamera.DownloadReady -= CanonCamera_DownloadReady;

                canonCamera.CloseSession();
            }

            // Abriendo la sesión.
            canonCamera.OpenSession();

            // Configurar la cámara para que guarde la foto en Tarjeta SD.
            canonCamera.SetSetting(Driver.Canon.PropertyID.SaveTo, (int)Driver.Canon.SaveTo.Host);

            // Configurar la cámara para que tenga capacidad disponible.
            canonCamera.SetCapacity(4096, int.MaxValue);

            // Configurar el evento que se encarga de Descargar la Foto.
            canonCamera.DownloadReady += CanonCamera_DownloadReady;

            // Hacer Foto.
            // camara.TakePhotoAsync();

            this.successActionCallback = successActionCallback;
            this.failActionCallback = failActionCallback;

            canonCamera.TakePhotoShutterAsync();
        }

        private void CanonCamera_DownloadReady(Driver.Canon.Camera sender, Driver.Canon.DownloadInfo Info)
        {
            try
            {
                var contentCaptureDirectoryPath = Providers.ConstantProvider.ContentCaptureDirectoryPath;

                if (!System.IO.Directory.Exists(contentCaptureDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(contentCaptureDirectoryPath);
                }

                Info.FileName = string.Format("{0}.JPG", targetCaptureImageId);

                sender.DownloadFile(Info, contentCaptureDirectoryPath);

                if (Info.IsRAW)
                {
                    // TODO Handle if photo in RAW format
                }

                canonCamera.DownloadReady -= CanonCamera_DownloadReady;
                canonCamera.CloseSession();

                System.Windows.Application.Current.Dispatcher.Invoke(successActionCallback);
            }
            catch (System.Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    failActionCallback?.Invoke(ex);
                });
            }

            successActionCallback = null;
            failActionCallback = null;
        }

        public override string ToString()
        {
            return string.Format("{0} (Canon ED API)", canonCamera.DeviceName);
        }
    }
}
