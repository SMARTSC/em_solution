namespace EM.Terminal.Dashboard.Factories.Model.Page
{
    public static class AlbumConfigurationPageModelFactory
    {
        public static Models.Page.AlbumConfigurationPageModel Build(Cloud.Backend.Api.Common.Models.AlbumDisplayModel model)
        {
            var result = new Models.Page.AlbumConfigurationPageModel();

            if (model != null)
            {
                result.Id = model.Id;
                result.Title = model.Title;
                result.Alias = model.Alias;
                result.IsEnabled = model.Enabled;
                result.IsPublic = model.Public;
                result.PhotoSessionCaptureCount = model.PhotoSessionCaptureCount;
                result.DefaultPictureContentCollageLayout = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptorById(model.DefaultPictureContentCollageLayout.Id);
                result.DefaultPhotoSessionContentCollageLayout = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptorById(model.DefaultPhotoSessionContentCollageLayout.Id);
                result.Description = model.Description;

                if (model.Overlay != null)
                {
                    result.OverlayImageSource = Xamarin.Forms.ImageSource.FromUri(UriFactory.BuildImageThumbnailUri(model.Overlay.ThumbnailId));
                }
            }

            return result;
        }
    }
}
