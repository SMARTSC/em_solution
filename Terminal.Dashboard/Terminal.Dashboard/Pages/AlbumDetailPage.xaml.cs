using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class AlbumDetailPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.AlbumDetailPageModel>
    {
        private Cloud.Backend.Api.Common.Models.EventDisplayModel @event;
        private System.Action albumChangeCallbackAction;

        public AlbumDetailPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel model, Cloud.Backend.Api.Common.Models.EventDisplayModel @event, System.Action albumChangeCallbackAction)
            : base(new Models.Page.AlbumDetailPageModel { Album = model })
        {
            this.@event = @event;
            this.albumChangeCallbackAction = albumChangeCallbackAction;

            InitializeComponent();
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            albumChangeCallbackAction = null;
        }

        private void EditButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new AlbumConfigurationPage(BindingContextPageModel.Album, @event, AlbumChange_Callback));
        }

        private void DeleteButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Esta seguro que desea eliminar este Album?", "Eliminar", "Cancelar").ContinueWith(dialogTask =>
            {
                if (!dialogTask.Result) return;

                var client = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

                BindingContextPageModel.IsBusy = true;

                client.Delete(BindingContextPageModel.Album.Id).ContinueWith(apiConnectTask =>
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

                        albumChangeCallbackAction?.Invoke();

                        this.NavigationPopPageAsync();
                    });
                });
            });
        }

        private void ManageContentsButton_Clicked(object sender, System.EventArgs eventArgs)
        {

        }

        private void ManageSecurityLocksButton_Clicked(object sender, System.EventArgs eventArgs)
        {

        }

        private void ManagePrintLayoutsButton_Clicked(object sender, System.EventArgs eventArgs)
        {

        }

        private void AlbumChange_Callback()
        {
            var client = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

            BindingContextPageModel.IsBusy = true;

            client.GetAsync(BindingContextPageModel.Album.Id).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);

                        return;
                    }

                    BindingContextPageModel.IsBusy = false;

                    BindingContextPageModel.Album = task.Result;
                });
            });

            albumChangeCallbackAction?.Invoke();
        }
    }
}