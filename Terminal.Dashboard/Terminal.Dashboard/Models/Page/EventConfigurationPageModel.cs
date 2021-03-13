namespace EM.Terminal.Dashboard.Models.Page
{
    public class EventConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string name;
        private Xamarin.Forms.ImageSource logoImageSource;
        private Xamarin.Forms.ImageSource overlayImageSource;
        private string description;
        private System.DateTime startAtDate;
        private System.TimeSpan startAtTime;
        private System.DateTime endAtDate;
        private System.TimeSpan endAtTime;
        private string placeName;

        public System.Guid? Id { get; set; }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public Xamarin.Forms.ImageSource LogoImageSource { get => logoImageSource; set => SetProperty(ref logoImageSource, value); }

        public string LogoContentType { get; set; }

        public Xamarin.Forms.ImageSource OverlayImageSource { get => overlayImageSource; set => SetProperty(ref overlayImageSource, value); }

        public string OverlayContentType { get; set; }

        public string Description { get => description; set => SetProperty(ref description, value); }

        public System.DateTime StartAtDate { get => startAtDate; set => SetProperty(ref startAtDate, value); }

        public System.TimeSpan StartAtTime { get => startAtTime; set => SetProperty(ref startAtTime, value); }

        public System.DateTime EndAtDate { get => endAtDate; set => SetProperty(ref endAtDate, value); }

        public System.TimeSpan EndAtTime { get => endAtTime; set => SetProperty(ref endAtTime, value); }

        public string PlaceName { get => placeName; set => SetProperty(ref placeName, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            LogoImageSource = null;
            OverlayImageSource = null;
        }
    }
}
