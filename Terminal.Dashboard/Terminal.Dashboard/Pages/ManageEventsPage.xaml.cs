using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventsPageModel>
    {
        public ManageEventsPage() : base(new Models.Page.ManageEventsPageModel())
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            LoadItems();

            IsEnabled = true;
        }

        private void EventOverviewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new EventDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel, EventsChange_Callback));
        }

        private void AddToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.IsBusy = true;

            var client = new Cloud.Backend.Api.Connect.Clients.EventKindsApiClient(Managers.SessionManager.Instance);

            client.GetAsync().ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                    else
                    {
                        if (task.Result.Length == 0)
                        {
                            DisplayAlert("Aviso", string.Format("EL sistema no tiene Tipos de Evento definidos.{0}{0}Es necesario que se defina al menos uno de estos elementos antes de poder continuar.", System.Environment.NewLine), "Aceptar");

                            return;
                        }

                        this.NavigationPushPageAsync(new EventConfigurationPage(task.Result, EventsChange_Callback));
                    }
                });
            });            
        }

        private void EventsChange_Callback()
        {
            IsEnabled = false;
        }

        private void LoadItems(int page = 0)
        {
            BindingContextPageModel.IsLoading = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Managers.SessionManager.Instance);

                return await client.GetPaginateAsync(page);
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsLoading = false;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                    else
                    {
                        if (page == 0)
                        {
                            BindingContextPageModel.Items.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
                            BindingContextPageModel.Items.Add(item);
                        }
                    }
                });
            });
        }
    }
}