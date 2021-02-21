using Xamarin.Essentials;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventAlbumPictureContentsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventAlbumPictureContentsPageModel>
    {
        public ManageEventAlbumPictureContentsPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel album)
            : base(new Models.Page.ManageEventAlbumPictureContentsPageModel { IsBusy = true, Album = album })
        {
            InitializeComponent();

            (Resources["ImageMediaDisplayModelToThumbnailImageSourceConverter"] as Converters.ImageMediaDisplayModelToThumbnailImageSourceConverter).AlbumId = album.Id;

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            LoadItems();

            IsEnabled = true;
        }

        private void AddToolbarItem_Activated(object sender, System.EventArgs e)
        {
            if (BindingContextPageModel.IsBusy) return;

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(() =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var pickOptions = new PickOptions
                        {
                            FileTypes = new FilePickerFileType(
                                new System.Collections.Generic.Dictionary<DevicePlatform, System.Collections.Generic.IEnumerable<string>>
                                {
                                    { DevicePlatform.Android, new string[] { "image/jpeg"} },
                                    { DevicePlatform.iOS, new string[] { "public.image" } },
                                    { DevicePlatform.macOS, new string[] { "public.image" } },
                                    { DevicePlatform.UWP, new string[] { ".jpg", ".jpeg" } },
                                }),
                            PickerTitle = "Seleccione las capturas a importar"
                        };

                        var pickResult = await FilePicker.PickMultipleAsync(pickOptions);

                        if (pickResult != null)
                        {
                            var reloadRequired = false;

                            foreach (var pick in pickResult)
                            {
                                if (!reloadRequired)
                                {
                                    reloadRequired = true;
                                }

                                using (var imageFileStream = await pick.OpenReadAsync())
                                {
                                    var imageData = new byte[imageFileStream.Length];

                                    await imageFileStream.ReadAsync(imageData, 0, imageData.Length);

                                    var client = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(Managers.SessionManager.Instance);

                                    await client.CreateAsync(new Cloud.Backend.Api.Common.Models.PictureContentInputModel
                                    {
                                        Id = System.Guid.NewGuid(),
                                        Created = System.DateTime.Now,
                                        AlbumId = BindingContextPageModel.Album.Id,
                                        ContentData = imageData,
                                        ContentType = pick.ContentType,
                                        Enabled = true
                                    });
                                }
                            }

                            if (reloadRequired)
                            {
                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                                    LoadItems();
                                });
                            }
                        }
                    }
                    catch (System.Exception exception)
                    {
                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(exception);
                    }

                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void DeleteItemButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Esta seguro que desea eliminar este elemento?", "Eliminar", "Cancelar").ContinueWith(dialogTask =>
            {
                if (!dialogTask.Result) return;

                var client = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(Managers.SessionManager.Instance);

                var model = (sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Api.Common.Models.PictureContentDisplayModel;

                BindingContextPageModel.IsBusy = true;

                client.Delete(model.Id).ContinueWith(apiRequestTask =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        BindingContextPageModel.IsBusy = true;

                        if (apiRequestTask.Exception != null || apiRequestTask.IsCanceled)
                        {
                            BindingContextPageModel.IsBusy = false;

                            (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(apiRequestTask.Exception);

                            return;
                        }

                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                        LoadItems();
                    });
                });
            });
        }

        private void LoadItems(int page = 0)
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(Managers.SessionManager.Instance);

                return await client.FilterPaginateAsync(new Cloud.Backend.Api.Common.Models.Filter.PictureContentFilterModel { AlbumId = BindingContextPageModel.Album.Id }, page, 200);
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