namespace EM.Terminal.Dashboard.Converters
{
    public class PictureContentDisplayModelToThumbnailImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public System.Guid AlbumId { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var target = value as Cloud.Api.Common.Models.PictureContentDisplayModel;

            return Xamarin.Forms.ImageSource.FromUri(Factories.UriFactory.BuildAlbumImageThumbnailUri(AlbumId, target.Image.ThumbnailId));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
