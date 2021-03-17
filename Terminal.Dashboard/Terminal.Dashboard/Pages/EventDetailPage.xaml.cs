using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class EventDetailPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.EventDetailPageModel>
    {
        private System.Action eventChangeCallbackAction;

        public EventDetailPage(Cloud.Backend.Api.Common.Models.EventDisplayModel model, System.Action eventChangeCallbackAction)
            : base(new Models.Page.EventDetailPageModel { Event = model })
        {
            this.eventChangeCallbackAction = eventChangeCallbackAction;

            InitializeComponent();
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            eventChangeCallbackAction = null;
        }

        private void EditButton_Clicked(object sender, System.EventArgs eventArgs)
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
                        this.NavigationPushPageAsync(new EventConfigurationPage(BindingContextPageModel.Event, task.Result, EventChange_Callback));
                    }
                });
            });
        }

        private void DeleteButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Esta seguro que desea eliminar este Evento?", "Eliminar", "Cancelar").ContinueWith(dialogTask =>
            {
                if (!dialogTask.Result) return;

                var client = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Managers.SessionManager.Instance);

                BindingContextPageModel.IsBusy = true;

                client.Delete(BindingContextPageModel.Event.Id).ContinueWith(apiConnectTask =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        if (apiConnectTask.Exception != null || apiConnectTask.IsCanceled)
                        {
                            BindingContextPageModel.IsBusy = false;

                            (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(apiConnectTask.Exception);

                            return;
                        }

                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                        eventChangeCallbackAction?.Invoke();

                        this.NavigationPopPageAsync();
                    });
                });
            });
        }

        private void ManageAlbumsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new ManageEventAlbumsPage(BindingContextPageModel.Event));
        }
        
        private void EventChange_Callback()
        {
            var client = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Managers.SessionManager.Instance);

            BindingContextPageModel.IsBusy = true;

            client.GetAsync(BindingContextPageModel.Event.Id).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);

                        return;
                    }

                    BindingContextPageModel.IsBusy = false;

                    BindingContextPageModel.Event = task.Result;
                });
            });

            eventChangeCallbackAction?.Invoke();
        }
    }
}