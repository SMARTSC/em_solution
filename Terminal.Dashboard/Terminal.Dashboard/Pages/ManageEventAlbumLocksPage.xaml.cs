using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;
using System.Linq;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventAlbumLocksPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventAlbumLocksPageModel>
    {
        public ManageEventAlbumLocksPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel model)
            : base(new Models.Page.ManageEventAlbumLocksPageModel { Album = model })
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            Reload();
        }

        private void UsePasswordLockSwitch_Toggled(object sender, Xamarin.Forms.ToggledEventArgs eventArgs)
        {
            if (!IsEnabled) return;

            IsEnabled = false;

            if (!eventArgs.Value)
            {
                DisplayAlert("Confirmar", "Está seguro que desea desactivar esta configuración de seguridad para el Album?", "Desactivar", "Cancelar").ContinueWith(confirmDialogTask =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        if (!confirmDialogTask.Result)
                        {
                            UsePasswordLockSwitch.IsToggled = true;

                            IsEnabled = true;

                            return;
                        }                    

                        BindingContextPageModel.IsBusy = true;

                        var albumApiConnectClient = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

                        albumApiConnectClient.DeleteLockAsync(BindingContextPageModel.Album.Id, Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).ContinueWith(apiConnectTask =>
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                if (apiConnectTask.Exception != null || apiConnectTask.IsCanceled)
                                {
                                    (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(apiConnectTask.Exception);

                                    BindingContextPageModel.IsBusy = false;
                                }
                                else
                                {
                                    Reload();
                                }
                            });
                        });
                    });
                });
            }
            else
            {
                this.NavigationPushPageAsync(new AlbumPasswordLockConfigurationPage(BindingContextPageModel.Album, PasswordLockConfigurationChange_Callback));
            }
        }

        private void UsePasswordLockConfigButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new AlbumPasswordLockConfigurationPage(BindingContextPageModel.PasswordLockConfiguration.SerializedContext, BindingContextPageModel.Album, PasswordLockConfigurationChange_Callback));
        }

        private void PasswordLockConfigurationChange_Callback(bool reload)
        {
            if (!reload)
            {
                UsePasswordLockSwitch.IsToggled = BindingContextPageModel.PasswordLockConfiguration != null;

                return;
            }

            IsEnabled = !reload;
        }

        private void Reload()
        {
            var albumApiConnectClient = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

            albumApiConnectClient.GetLocksAsync(BindingContextPageModel.Album.Id).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);

                        this.NavigationPopPageAsync();
                    }
                    else
                    {
                        var passwordLock = task.Result.FirstOrDefault(item => item.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID);

                        if (passwordLock == null)
                        {
                            BindingContextPageModel.PasswordLockConfiguration = null;
                        }
                        else
                        {
                            BindingContextPageModel.PasswordLockConfiguration = passwordLock;
                        }

                        BindingContextPageModel.IsBusy = false;

                        IsEnabled = true;
                    }
                });
            });
        }
    }
}