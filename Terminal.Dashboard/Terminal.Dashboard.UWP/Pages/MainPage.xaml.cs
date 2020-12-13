namespace EM.Terminal.Dashboard.UWP.Pages
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new Dashboard.App());
        }
    }
}