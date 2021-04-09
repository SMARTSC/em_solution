namespace EM.Terminal.PhotoMirror.Devices.Facade
{
    public interface ICaptureDeviceFacade
    {
        void Capture(System.Guid captureImageId, System.Action successActionCallback, System.Action<System.Exception> failActionCallback);
    }
}
