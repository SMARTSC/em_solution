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

            if ((BindingContext as Models.Page.SessionConfigurationPageModel).ActiveEvents != null) return;

            LoadActiveEvents();
        }

        private void EventOverviewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            (BindingContext as Models.Page.SessionConfigurationPageModel).TargetEvent = (sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel;

            presentingPage = Children[2];

            CurrentPage = presentingPage;
        }

        private void AlbumOverviewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            sessionManager.TargetEvent = (BindingContext as Models.Page.SessionConfigurationPageModel).TargetEvent;
            sessionManager.TargetAlbum = (sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.AlbumDisplayModel;

            presentingPage = Children[3];

            CurrentPage = presentingPage;
        }

        private void LoadActiveEvents()
        {
            var bindingContextPageModel = BindingContext as Models.Page.SessionConfigurationPageModel;

            var eventsApiConnectClient = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Xamarin.Forms.DependencyService.Get<SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade>());

            bindingContextPageModel.IsBusy = true;

            // TODO Get only active events!
            eventsApiConnectClient.GetAsync().ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    bindingContextPageModel.IsBusy = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        Xamarin.Forms.DependencyService.Get<Common.Helpers.Facade.IExceptionHandleHelperFacade>().HandleException(task.Exception, LoadActiveEvents);

                        return;
                    }

                    bindingContextPageModel.ActiveEvents = new System.Collections.ObjectModel.ObservableCollection<Cloud.Backend.Api.Common.Models.EventDisplayModel>();

                    foreach (var item in task.Result)
                    {
                        bindingContextPageModel.ActiveEvents.Add(item);
                    }
                });
            });
        }
    }
}