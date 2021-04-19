namespace EM.Terminal.PrintCenter.Converters
{
    public class PrintLayoutDescriptorToPictureContentPrintableViewConverter : Xamarin.Forms.IValueConverter
    {
        public Pages.IPictureContentPrintableViewPresenter Presenter { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            if (!(value is Extend.Layout.Descriptors.IPrintLayoutDescriptor)) return null;

            return Xamarin.Forms.DependencyService.Get<Providers.PrintLayoutViewProvider>().GetPictureContentPrintLayoutView(Presenter.PictureContent, true, value as Extend.Layout.Descriptors.IPrintLayoutDescriptor);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
