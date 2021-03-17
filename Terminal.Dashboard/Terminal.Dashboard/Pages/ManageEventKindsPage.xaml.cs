using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventKindsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventKindsPageModel>
    {
        public ManageEventKindsPage() : base(new Models.Page.ManageEventKindsPageModel { IsBusy = true })
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

        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new EventKindConfigurationPage(eventArgs.Item as Cloud.Backend.Api.Common.Models.EventKindDisplayModel, EventKindsChange_Callback));
        }

        private void EditMenuItem_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new EventKindConfigurationPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventKindDisplayModel, EventKindsChange_Callback));
        }

        private void DeleteMenuItem_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Confirmar", "Esta seguro que desea eliminar este Tipo Evento?", "Eliminar", "Cancelar").ContinueWith(dialogTask =>
            {
                if (!dialogTask.Result) return;

                var client = new Cloud.Backend.Api.Connect.Clients.EventKindsApiClient(Managers.SessionManager.Instance);

                BindingContextPageModel.IsBusy = true;

                client.Delete(((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventKindDisplayModel).Id).ContinueWith(apiConnectTask =>
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

                        LoadItems();
                    });
                });
            });
        }

        private void AddToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new EventKindConfigurationPage(EventKindsChange_Callback));
        }

        private void EventKindsChange_Callback()
        {
            IsEnabled = false;
        }

        private void LoadItems()
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.EventKindsApiClient(Managers.SessionManager.Instance);

                return await client.GetAsync();
            }).ContinueWith(task =>
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
                        BindingContextPageModel.Items.Clear();

                        foreach (var item in task.Result)
                        {
                            BindingContextPageModel.Items.Add(item);
                        }
                    }
                });
            });
        }
    }
}