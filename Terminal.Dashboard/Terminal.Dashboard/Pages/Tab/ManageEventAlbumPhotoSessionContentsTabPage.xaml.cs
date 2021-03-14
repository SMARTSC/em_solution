using Xamarin.Essentials;

namespace EM.Terminal.Dashboard.Pages.Tab
{
    public partial class ManageEventAlbumPhotoSessionContentsTabPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.Tab.ManageEventAlbumPhotoSessionContentsTabPageModel>
    {
        public ManageEventAlbumPhotoSessionContentsTabPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel album)
            : base(new Models.Page.Tab.ManageEventAlbumPhotoSessionContentsTabPageModel { IsBusy = true, Album = album })
        {
            InitializeComponent();

            (Resources["PhotoSessionContentDisplayModelImageIndexToThumbnailImageSourceConverter"] as Converters.PhotoSessionContentDisplayModelImageIndexToThumbnailImageSourceConverter).AlbumId = album.Id;

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
                            var groupedBySession = new System.Collections.Generic.Dictionary<System.Guid, System.Collections.Generic.List<FileResult>>();

                            var photoSessionContentId = System.Guid.NewGuid();

                            foreach (var pick in pickResult)
                            {
                                var fileRawName = System.IO.Path.GetFileNameWithoutExtension(pick.FileName);

                                try
                                {
                                    var parceableGuids = fileRawName.Split('_');

                                    if (parceableGuids.Length > 1)
                                    {
                                        photoSessionContentId = System.Guid.Parse(parceableGuids[1]);
                                    }
                                }
                                catch { }

                                try
                                {
                                    groupedBySession[photoSessionContentId].Add(pick);
                                }
                                catch
                                {
                                    groupedBySession.Add(photoSessionContentId, new System.Collections.Generic.List<FileResult> { pick });
                                }
                            }

                            if (groupedBySession.Count > 0)
                            {
                                foreach (var group in groupedBySession)
                                {
                                    var client = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(Managers.SessionManager.Instance);

                                    await client.CreateAsync(new Cloud.Backend.Api.Common.Models.PhotoSessionContentInputModel
                                    {
                                        Id = group.Key,
                                        Created = System.DateTime.Now,
                                        AlbumId = BindingContextPageModel.Album.Id,
                                        CaptureCount = group.Value.Count,                                        
                                        Enabled = true
                                    });

                                    foreach (var pick in group.Value)
                                    {
                                        using (var imageFileStream = await pick.OpenReadAsync())
                                        {
                                            var imageData = new byte[imageFileStream.Length];

                                            await imageFileStream.ReadAsync(imageData, 0, imageData.Length);

                                            await client.AgregateAsync(group.Key, new Cloud.Backend.Api.Common.Models.ImageMediaInputModel
                                            {
                                                ContentData = imageData,
                                                ContentType = pick.ContentType,
                                            });
                                        }
                                    }
                                }

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

                var client = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(Managers.SessionManager.Instance);

                var model = (sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Api.Common.Models.PhotoSessionContentDisplayModel;

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
                var client = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(Managers.SessionManager.Instance);

                return await client.FilterPaginateAsync(new Cloud.Backend.Api.Common.Models.Filter.PhotoSessionContentFilterModel { AlbumId = BindingContextPageModel.Album.Id }, page, 200);
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