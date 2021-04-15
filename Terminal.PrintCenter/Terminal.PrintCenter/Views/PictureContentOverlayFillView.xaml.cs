namespace EM.Terminal.PrintCenter.Views
{
    public partial class PictureContentOverlayFillView : Xamarin.Forms.ContentView
    {
        public PictureContentOverlayFillView(Cloud.Api.Common.Models.PictureContentDisplayModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }
    }
}