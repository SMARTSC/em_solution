namespace EM.Terminal.Dashboard.Converters
{
    public class CollageLayoutDescriptorToOverviewImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var descriptor = value as Extend.Layout.Descriptors.IPrintLayoutDescriptor;

            return Xamarin.Forms.ImageSource.FromResource(string.Format("EM.Terminal.Dashboard.Resources.Image.{0}.jpg", descriptor.Id.ToString().ToUpper()), GetType().Assembly);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
