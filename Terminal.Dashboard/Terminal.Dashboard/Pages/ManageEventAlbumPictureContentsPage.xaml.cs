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
                        string[] fileSelector = null;

                        switch (Xamarin.Forms.Device.RuntimePlatform)
                        {
                            case Xamarin.Forms.Device.Android:
                                fileSelector = new string[] { "image/*" };

                                break;

                            case Xamarin.Forms.Device.UWP:
                                fileSelector = new string[] { ".jpg", ".jpeg" };

                                break;
                        }

                        var targetFileData = await Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector);

                        if (targetFileData != null)
                        {
                            using (var imageFileStream = targetFileData.GetStream())
                            {
                                var imageData = new byte[imageFileStream.Length];

                                await imageFileStream.ReadAsync(imageData, 0, imageData.Length);

                                var client = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(Managers.SessionManager.Instance);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                                client.CreateAsync(new Cloud.Backend.Api.Common.Models.PictureContentInputModel
                                {
                                    Id = System.Guid.NewGuid(),
                                    Created = System.DateTime.Now,
                                    AlbumId = BindingContextPageModel.Album.Id,
                                    ContentData = targetFileData.DataArray,
                                    ContentType = "image/jpeg",
                                    Enabled = true
                                }).ContinueWith(task =>
                                {
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        if (task.Exception != null || task.IsCanceled)
                                        {
                                            BindingContextPageModel.IsBusy = false;

                                            (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);

                                            return;
                                        }

                                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                                        LoadItems();
                                    });
                                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            }
                        }
                        else
                        {
                            BindingContextPageModel.IsBusy = false;
                        }
                    }
                    catch
                    {
                        BindingContextPageModel.IsBusy = false;
                    }
                });
            });
        }

        private void LoadItems(int page = 0)
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(Managers.SessionManager.Instance);

                return await client.FilterPaginateAsync(new Cloud.Backend.Api.Common.Models.Filter.PictureContentFilterModel { AlbumId = BindingContextPageModel.Album.Id }, page);
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