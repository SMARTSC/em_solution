namespace EM.Terminal.PrintCenter.Views
{
    public partial class PhotoSessionContentOverlayFillView : Xamarin.Forms.ContentView
    {
        public PhotoSessionContentOverlayFillView(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model)
        {
            InitializeComponent();

            (Resources["PhotoSessionContentDisplayModelImageIndexToThumbnailImageSourceConverter"] as Common.Converters.PhotoSessionContentDisplayModelImageIndexToThumbnailImageSourceConverter).AlbumId = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().TargetAlbum.Id;

            BindingContext = model;
        }
    }
}