using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class SessionConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.CarouselPage
    {
        private Xamarin.Forms.ContentPage presentingPage;

        public SessionConfigurationPage()
        {
            InitializeComponent();

            BindingContext = new Models.Page.SessionConfigurationPageModel();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void CarouselPage_CurrentPageChanged(object sender, System.EventArgs e)
        {
            if (presentingPage == null)
            {
                presentingPage = CurrentPage;

                return;
            }

            if (presentingPage != CurrentPage)
            {
                CurrentPage = presentingPage;
            }
        }

        private void UserAuthenticationChildPage_Authenticated(object sender, System.EventArgs eventArgs)
        {
            presentingPage = Children[1];

            CurrentPage = presentingPage;
        }

        private void EventOverviewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            (BindingContext as Models.Page.SessionConfigurationPageModel).Event = (sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel;

            presentingPage = Children[2];

            CurrentPage = presentingPage;
        }

        private void AlbumOverviewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            sessionManager.TargetEvent = (BindingContext as Models.Page.SessionConfigurationPageModel).Event;
            sessionManager.TargetAlbum = (sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.AlbumDisplayModel;

            this.NavigationPopModalPageAsync();
        }
    }
}