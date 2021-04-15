namespace EM.Terminal.PrintCenter.Converters
{
    public class ContentDisplayModelToContentConcreteFillViewConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            if (!(value is Cloud.Api.Common.Models.ContentDisplayModel)) return value;

            if (value is Cloud.Api.Common.Models.PictureContentDisplayModel) return new Views.PictureContentOverlayFillView(value as Cloud.Api.Common.Models.PictureContentDisplayModel);

            if (value is Cloud.Api.Common.Models.PhotoSessionContentDisplayModel) return new Views.PhotoSessionContentOverlayFillView(value as Cloud.Api.Common.Models.PhotoSessionContentDisplayModel);

            // TODO Handle others content kind!

            throw new System.InvalidOperationException();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
