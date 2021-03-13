using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;
using Xamarin.Essentials;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class AlbumConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.AlbumConfigurationPageModel>
    {
        private bool pageModelHasChanges;

        private Cloud.Backend.Api.Common.Models.EventDisplayModel @event;
        private System.Action successfulCompleteCallbackAction;

        public AlbumConfigurationPage(Cloud.Backend.Api.Common.Models.EventDisplayModel @event, System.Action successfulCompleteCallbackAction)
            : this(null, @event, successfulCompleteCallbackAction)
        {
        }

        public AlbumConfigurationPage(Cloud.Backend.Api.Common.Models.AlbumDisplayModel model, Cloud.Backend.Api.Common.Models.EventDisplayModel @event, System.Action successfulCompleteCallbackAction)
            : base(Factories.Model.Page.AlbumConfigurationModelPageFactory.Build(model))
        {
            this.@event = @event;
            this.successfulCompleteCallbackAction = successfulCompleteCallbackAction;

            InitializeComponent();

            BindingContextPageModel.PropertyChanged += PageModel_PropertyChanged;

            if (model == null)
            {
                Title = "Nuevo Album";

                return;
            }

            Title = "Modificar Album";
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

            successfulCompleteCallbackAction = null;
        }

        private void PageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            pageModelHasChanges = true;
        }

        private void ApplyToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                this.NavigationPopPageAsync();

                return;
            }

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.AlbumsApiClient(Managers.SessionManager.Instance);

                var model = new Cloud.Backend.Api.Common.Models.AlbumInputModel
                {
                    EventId = @event.Id,
                    Title = BindingContextPageModel.Title,
                    Alias = BindingContextPageModel.Alias,
                    Enabled = BindingContextPageModel.IsEnabled,
                    Public = BindingContextPageModel.IsPublic,
                    PhotoSessionCaptureCount = BindingContextPageModel.PhotoSessionCaptureCount,
                    DefaultPictureContentCollageLayoutId = BindingContextPageModel.DefaultPictureContentCollageLayout.Id,
                    DefaultPhotoSessionContentCollageLayoutId = BindingContextPageModel.DefaultPhotoSessionContentCollageLayout.Id,
                    Description = BindingContextPageModel.Description
                };

                if (BindingContextPageModel.OverlayImageSource != null && BindingContextPageModel.OverlayImageSource is Xamarin.Forms.StreamImageSource)
                {
                    using (var stream = await (BindingContextPageModel.OverlayImageSource as Xamarin.Forms.StreamImageSource).Stream.Invoke(System.Threading.CancellationToken.None))
                    {
                        model.Overlay = new Cloud.Backend.Api.Common.Models.ImageMediaInputModel
                        {
                            ContentData = (stream as System.IO.MemoryStream).ToArray(),
                            ContentType = BindingContextPageModel.OverlayContentType
                        };
                    }
                }

                if (BindingContextPageModel.Id != null)
                {
                    model.Id = BindingContextPageModel.Id.Value;

                    await client.EditAsync(model);
                }
                else
                {
                    await client.CreateAsync(model);
                }
            }).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception == null && !task.IsCanceled)
                    {
                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Cambios aplicados con éxito.");

                        successfulCompleteCallbackAction?.Invoke();

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
                this.NavigationPopPageAsync();

                return;
            }

            DisplayAlert("Confirmar", "Está seguro que desea regresar sin guardar los cambios?", "Regresar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.NavigationPopPageAsync();
                });
            });
        }

        private void OverlayImageGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(() =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
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
                        PickerTitle = "Seleccione la image a importar"
                    };

                    var pickResult = await FilePicker.PickAsync(pickOptions);

                    if (pickResult != null)
                    {
                        using (var imageFileStream = await pickResult.OpenReadAsync())
                        {
                            var imageData = new byte[imageFileStream.Length];

                            await imageFileStream.ReadAsync(imageData, 0, imageData.Length);

                            BindingContextPageModel.OverlayImageSource = Xamarin.Forms.ImageSource.FromStream(() => new System.IO.MemoryStream(imageData));
                            BindingContextPageModel.OverlayContentType = pickResult.ContentType;
                        }
                    }

                    BindingContextPageModel.IsBusy = false;
                });
            });
        }

        private void PicturePrintLayoutViewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new CollageLayoutPickPage(PicturePrintLayoutPicked_Callback));
        }

        private void PhotoSessionPrintLayoutViewGestureRecognizer_Tapped(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPushPageAsync(new CollageLayoutPickPage(PhotoSessionPrintLayoutPicked_Callback));
        }

        private void PicturePrintLayoutPicked_Callback(Extend.Layout.Descriptors.Facade.ICollageLayoutDescriptorFacade printLayout)
        {
            BindingContextPageModel.DefaultPictureContentCollageLayout = printLayout;
        }

        private void PhotoSessionPrintLayoutPicked_Callback(Extend.Layout.Descriptors.Facade.ICollageLayoutDescriptorFacade printLayout)
        {
            BindingContextPageModel.DefaultPhotoSessionContentCollageLayout = printLayout;
        }
    }
}