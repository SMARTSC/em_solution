namespace EM.Terminal.PrintCenter.Views.Layout.Print
{
    public partial class Default_0003_1_3x2_PrintLayoutView : BasePictureContentPrintLayoutView
    {
        public Default_0003_1_3x2_PrintLayoutView(Cloud.Api.Common.Models.PictureContentDisplayModel model, System.Action presentationReadyCallbackAction, System.Action presentationFailCallbackAction)
            : base(new Extend.Layout.Pioneer.Descriptors.Default_0003_1_3x2_PrintLayoutDescriptor(), presentationReadyCallbackAction, presentationFailCallbackAction)
        {
            InitializeComponent();

            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            PictureContentImage.Source = Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageThumbnailUri(sessionManager.TargetAlbum.Id, model.Image.ThumbnailId));
            PictureContentImage.FadeAnimationEnabled = true;

            EventLogoImage.Source = Common.Factories.UriFactory.BuildImageUri(sessionManager.TargetEvent.Logo.Id);
            EventLogoImage.FadeAnimationEnabled = true;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            PictureContentImage.Source = null;
            EventLogoImage.Source = null;

            Content = null;
        }

        private void PictureContentImage_Finish(object sender, FFImageLoading.Forms.CachedImageEvents.FinishEventArgs e)
        {

        }
    }
}