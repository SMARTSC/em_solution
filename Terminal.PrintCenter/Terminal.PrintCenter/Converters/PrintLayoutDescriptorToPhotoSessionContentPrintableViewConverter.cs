namespace EM.Terminal.PrintCenter.Converters
{
    public class PrintLayoutDescriptorToPhotoSessionContentPrintableViewConverter : Xamarin.Forms.IValueConverter
    {
        public Pages.IPhotoSessionContentPrintableViewPresenter Presenter { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            if (!(value is Extend.Layout.Descriptors.IPrintLayoutDescriptor)) return null;

            return Xamarin.Forms.DependencyService.Get<Providers.PrintLayoutViewProvider>().GetPhotoSessionContentPrintLayoutView(Presenter.PhotoSessionContent, Presenter.Permutation, value as Extend.Layout.Descriptors.IPrintLayoutDescriptor);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
