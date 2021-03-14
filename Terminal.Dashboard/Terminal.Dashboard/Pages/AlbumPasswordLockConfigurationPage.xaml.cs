using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class AlbumPasswordLockConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.AlbumPasswordLockConfigurationPageModel>
    {
        private bool pageModelHasChanges;

        private readonly bool isNew;

        private Cloud.Backend.Api.Common.Models.AlbumDisplayModel album;
        private System.Action<bool> completeCallbackAction;

        public AlbumPasswordLockConfigurationPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel album, System.Action<bool> completeCallbackAction)
            : this(null, album, completeCallbackAction)
        {
        }

        public AlbumPasswordLockConfigurationPage(string serializedContext, Cloud.Backend.Api.Common.Models.AlbumDisplayModel album, System.Action<bool> completeCallbackAction) : base(new Models.Page.AlbumPasswordLockConfigurationPageModel { Password = serializedContext })
        {
            this.album = album;
            this.completeCallbackAction = completeCallbackAction;

            isNew = serializedContext == null;

            InitializeComponent();

            BindingContextPageModel.PropertyChanged += PageModel_PropertyChanged;
        }

        protected override bool OnBackButtonPressed()
        {
            CancelToolbarItem_Clicked(this, System.EventArgs.Empty);

            return true;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            BindingContextPageModel.PropertyChanged -= PageModel_PropertyChanged;

            album = null;
            completeCallbackAction = null;
        }

        private void PageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            pageModelHasChanges = true;
        }

        private void ApplyToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                if (isNew) completeCallbackAction?.Invoke(false);

                this.NavigationPopPageAsync();

                return;
            }

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var albumApiConnectClient = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

                var model = new Cloud.Backend.Api.Common.Models.AlbumLockInputModel
                {
                    AlbumId = album.Id,
                    LockId = Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID,
                    SerializedContext = BindingContextPageModel.Password
                };

                if (!isNew)
                {
                    await albumApiConnectClient.EditLockAsync(model);
                }
                else
                {
                    await albumApiConnectClient.CreateLockAsync(model);
                }
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception == null && !task.IsCanceled)
                    {
                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                        completeCallbackAction?.Invoke(true);

                        this.NavigationPopPageAsync();
                    }
                    else
                    {
                        BindingContextPageModel.IsBusy = false;

                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                });
            });
        }

        private void CancelToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                completeCallbackAction?.Invoke(false);

                this.NavigationPopPageAsync();

                return;
            }

            DisplayAlert("Confirmar", "Está seguro que desea regresar sin guardar los cambios?", "Regresar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    completeCallbackAction?.Invoke(false);

                    this.NavigationPopPageAsync();
                });
            });
        }
    }
}