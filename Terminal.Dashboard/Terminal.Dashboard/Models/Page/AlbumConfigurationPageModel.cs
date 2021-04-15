namespace EM.Terminal.Dashboard.Models.Page
{
    public class AlbumConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string title;
        private string alias;
        private bool isEnabled;
        private bool isPublic;
        private Extend.Layout.Descriptors.IPrintLayoutDescriptor defaultPictureContentCollageLayoutDescriptor;
        private Extend.Layout.Descriptors.IPrintLayoutDescriptor defaultPhotoSessionContentCollageLayoutDescriptor;
        private Xamarin.Forms.ImageSource overlayImageSource;
        private string description;

        public System.Guid? Id { get; set; }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public string Alias { get => alias; set => SetProperty(ref alias, value); }

        public bool IsEnabled { get => isEnabled; set => SetProperty(ref isEnabled, value); }

        public bool IsPublic { get => isPublic; set => SetProperty(ref isPublic, value); }

        public int PhotoSessionCaptureCount { get; set; }

        public Extend.Layout.Descriptors.IPrintLayoutDescriptor DefaultPictureContentCollageLayout { get => defaultPictureContentCollageLayoutDescriptor; set => SetProperty(ref defaultPictureContentCollageLayoutDescriptor, value); }

        public Extend.Layout.Descriptors.IPrintLayoutDescriptor DefaultPhotoSessionContentCollageLayout { get => defaultPhotoSessionContentCollageLayoutDescriptor; set => SetProperty(ref defaultPhotoSessionContentCollageLayoutDescriptor, value); }

        public Xamarin.Forms.ImageSource OverlayImageSource { get => overlayImageSource; set => SetProperty(ref overlayImageSource, value); }

        public string OverlayContentType { get; set; }

        public string Description { get => description; set => SetProperty(ref description, value); }
    }
}
