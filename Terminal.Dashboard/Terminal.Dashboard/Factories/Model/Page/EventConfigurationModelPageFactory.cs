namespace EM.Terminal.Dashboard.Factories.Model.Page
{
    public static class EventConfigurationModelPageFactory 
    {
        public static Models.Page.EventConfigurationPageModel Build(Cloud.Backend.Api.Common.Models.EventDisplayModel model)
        {
            var result = new Models.Page.EventConfigurationPageModel();

            if (model != null)
            {
                result.Id = model.Id;
                result.Name = model.Name;
                result.StartAtDate = model.Start;
                result.EndAtDate = model.End;
                result.PlaceName = model.PlaceName;
                result.Description = model.Description;

                result.LogoImageSource = Xamarin.Forms.ImageSource.FromUri(UriFactory.BuildImageUri(model.Logo.Id));

                if (model.Overlay != null)
                {
                    result.OverlayImageSource = Xamarin.Forms.ImageSource.FromUri(UriFactory.BuildImageThumbnailUri(model.Overlay.ThumbnailId));
                }
            }
                
            return result;
        }
    }
}
