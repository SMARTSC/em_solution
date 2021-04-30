namespace EM.Terminal.PrintCenter.Views.Layout.Print
{
    public partial class Default_0003_1_3x2_PrintLayoutView : BasePictureContentPrintLayoutView
    {
        public Default_0003_1_3x2_PrintLayoutView(Cloud.Api.Common.Models.PictureContentDisplayModel model, bool forOverview, System.Action presentationReadyCallbackAction, System.Action presentationFailCallbackAction)
            : base(new Extend.Layout.Pioneer.Descriptors.Default_0003_1_3x2_PrintLayoutDescriptor(), forOverview, presentationReadyCallbackAction, presentationFailCallbackAction)
        {
            InitializeComponent();

            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            PictureContentImage.Source = forOverview ?
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageThumbnailUri(sessionManager.TargetAlbum.Id, model.Image.ThumbnailId)) :
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageUri(sessionManager.TargetAlbum.Id, model.Image.Id));
            PictureContentImage.FadeAnimationEnabled = forOverview;
            PictureContentImage.DownsampleToViewSize = forOverview;

            EventLogoImage.Source = Common.Factories.UriFactory.BuildImageUri(sessionManager.TargetEvent.Logo.Id);
            EventLogoImage.FadeAnimationEnabled = forOverview;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            PictureContentImage.Source = null;
            EventLogoImage.Source = null;

            Content = null;

            System.GC.Collect();
        }
    }
}