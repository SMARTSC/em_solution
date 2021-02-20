namespace EM.Terminal.Dashboard.Converters
{
    public class ImageMediaDisplayModelToThumbnailImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public System.Guid AlbumId { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var target = value as Cloud.Api.Common.Models.PictureContentDisplayModel;

            return Xamarin.Forms.ImageSource.FromUri(new System.Uri(string.Format("{1}://{0}/business/media/album/{2}/thumbnail/{3}", Managers.SessionManager.Instance.Server, Managers.SessionManager.CLOUD_CONNECTION_SCHEMA, AlbumId, target.Image.ThumbnailId)));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
