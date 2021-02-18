namespace EM.Terminal.Dashboard.Converters
{
    public class EventDisplayModelToLogoImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var model = value as Cloud.Backend.Api.Common.Models.EventDisplayModel;

            return Xamarin.Forms.ImageSource.FromUri(new System.Uri(string.Format("https://{0}/business/media/image/{1}", Managers.SessionManager.Instance.Server, model.Logo.Id)));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}