namespace EM.Terminal.PhotoMirror.Models
{
    public class ContentHandlingConfigurationContextModel
    {
        public Cloud.Backend.Api.Common.Models.EventDisplayModel[] Events { get; set; }

        public System.Collections.Generic.List<Devices.Facade.ICaptureDeviceFacade> CaptureDevices { get; set; }
    }
}
