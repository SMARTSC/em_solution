namespace EM.Terminal.Dashboard.Converters
{
    public class ImageMediaDisplayModelToThumbnailImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public System.Guid AlbumId { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var target = value as Cloud.Api.Common.Models.PictureContentDisplayModel;

            return Xamarin.Forms.ImageSource.FromUri(new System.Uri(string.Format("https://{0}/business/media/album/{1}/thumbnail/{2}", Managers.SessionManager.Instance.Server, AlbumId, target.Image.ThumbnailId)));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
