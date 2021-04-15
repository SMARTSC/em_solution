namespace EM.Terminal.PrintCenter.Views
{
    public partial class PhotoSessionContentOverlayFillView : Xamarin.Forms.ContentView
    {
        public PhotoSessionContentOverlayFillView(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }
    }
}