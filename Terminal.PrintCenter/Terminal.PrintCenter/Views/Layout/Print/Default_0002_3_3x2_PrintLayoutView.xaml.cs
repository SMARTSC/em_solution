namespace EM.Terminal.PrintCenter.Views.Layout.Print
{
    public partial class Default_0002_3_3x2_PrintLayoutView : BasePhotoSessionContentPrintLayoutView
    {
        private bool firstImageReady;
        private bool secoundImageReady;
        private bool thirdImageReady;

        public Default_0002_3_3x2_PrintLayoutView()
        {
            throw new System.InvalidOperationException();
        }

        public Default_0002_3_3x2_PrintLayoutView(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model, int[] permutation, bool forOverview, System.Action presentationReadyCallbackAction, System.Action presentationFailCallbackAction) : base(permutation, forOverview, presentationReadyCallbackAction, presentationFailCallbackAction)
        {
            firstImageReady = false;
            secoundImageReady = false;
            thirdImageReady = false;

            InitializeComponent();

            var firstImage = model.Images[permutation[0]];
            var secoundImage = model.Images[permutation[1]];
            var thirdImage = model.Images[permutation[2]];

            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            FirstPhotoSessionContentImage.Source = forOverview ?
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageThumbnailUri(sessionManager.TargetAlbum.Id, firstImage.ThumbnailId)) :
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageUri(sessionManager.TargetAlbum.Id, firstImage.Id));
            FirstPhotoSessionContentImage.FadeAnimationEnabled = forOverview;

            SecoundPhotoSessionContentImage.Source = forOverview ?
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageThumbnailUri(sessionManager.TargetAlbum.Id, secoundImage.ThumbnailId)) :
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageUri(sessionManager.TargetAlbum.Id, secoundImage.Id));
            SecoundPhotoSessionContentImage.FadeAnimationEnabled = forOverview;

            ThirdPhotoSessionContentImage.Source = forOverview ?
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageThumbnailUri(sessionManager.TargetAlbum.Id, thirdImage.ThumbnailId)) :
                Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageUri(sessionManager.TargetAlbum.Id, thirdImage.Id));
            ThirdPhotoSessionContentImage.FadeAnimationEnabled = forOverview;

            EventLogoImage.Source = Common.Factories.UriFactory.BuildImageUri(sessionManager.TargetEvent.Logo.Id);
            EventLogoImage.FadeAnimationEnabled = forOverview;
        }

        private void CachedImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            (sender as FFImageLoading.Forms.CachedImage).Success -= CachedImage_Success;
            (sender as FFImageLoading.Forms.CachedImage).Error -= CachedImage_Error;

            if (sender == FirstPhotoSessionContentImage) firstImageReady = true;
            if (sender == SecoundPhotoSessionContentImage) secoundImageReady = true;
            if (sender == ThirdPhotoSessionContentImage) thirdImageReady = true;

            if (!firstImageReady || !secoundImageReady || !thirdImageReady) return;

            presentationReadyCallbackAction?.Invoke();
        }

        private void CachedImage_Error(object sender, FFImageLoading.Forms.CachedImageEvents.ErrorEventArgs e)
        {
            FirstPhotoSessionContentImage.Success -= CachedImage_Success;
            FirstPhotoSessionContentImage.Error -= CachedImage_Error;

            SecoundPhotoSessionContentImage.Success -= CachedImage_Success;
            SecoundPhotoSessionContentImage.Error -= CachedImage_Error;

            ThirdPhotoSessionContentImage.Success -= CachedImage_Success;
            ThirdPhotoSessionContentImage.Error -= CachedImage_Error;

            presentationFailCallbackAction?.Invoke();
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            FirstPhotoSessionContentImage.Source = null;
            SecoundPhotoSessionContentImage.Source = null;
            ThirdPhotoSessionContentImage.Source = null;
            EventLogoImage.Source = null;
        }
    }
}