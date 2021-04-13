using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class StartUpPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.ContentPage
    {
        public StartUpPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().HasAuthenticatedSession)
            {
                this.NavigationPushModalPageAsync(new SessionConfigurationPage());

                return;
            }

            // TODO Present Session info in passive mode until interaction!
        }
    }
}