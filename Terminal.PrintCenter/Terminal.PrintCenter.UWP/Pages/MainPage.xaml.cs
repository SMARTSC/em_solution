namespace EM.Terminal.PrintCenter.UWP.Pages
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new PrintCenter.App());
        }
    }
}
