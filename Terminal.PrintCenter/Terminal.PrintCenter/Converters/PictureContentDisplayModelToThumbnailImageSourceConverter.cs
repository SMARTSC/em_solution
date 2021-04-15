namespace EM.Terminal.PrintCenter.Converters
{
    public class PictureContentDisplayModelToThumbnailImageSourceConverter : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var target = value as Cloud.Api.Common.Models.PictureContentDisplayModel;

            return Xamarin.Forms.ImageSource.FromUri(Common.Factories.UriFactory.BuildAlbumImageThumbnailUri(Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().TargetAlbum.Id, target.Image.ThumbnailId));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
