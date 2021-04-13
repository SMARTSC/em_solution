namespace EM.Terminal.Common.Converters
{
    public class ImageMediaDisplayModelToThumbnailImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var model = value as Cloud.Api.Common.Models.ImageMediaDisplayModel;

            return Xamarin.Forms.ImageSource.FromUri(Factories.UriFactory.BuildImageThumbnailUri(model.ThumbnailId));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
