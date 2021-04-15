namespace EM.Terminal.PrintCenter.Converters
{
    public class ContentOverviewsCollectionViewHeightToContentOverviewWidthConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var target = (double)value;

            if (target <= 0) return target;

            return (target / 3 * 1.5 - 16) * Helpers.ResponsivePresentationHelper.ContentOverviewCollectionViewSnap;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
