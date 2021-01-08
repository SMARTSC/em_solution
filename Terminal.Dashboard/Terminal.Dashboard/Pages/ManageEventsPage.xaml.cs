using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventsPageModel>
    {
        private const string EVENT_OPTION_DETAIL = "Detalles";
        private const string EVENT_OPTION_EDIT_METADATA = "Modificar Metadatos";
        private const string EVENT_OPTION_MANAGE_ALBUMS = "Gestionar Albumes";
        private const string EVENT_OPTION_DELETE = "Eliminar";

        public ManageEventsPage() : base(new Models.Page.ManageEventsPageModel { IsBusy = true })
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

        private void EventOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayActionSheet("Opciones de Evento", "Cancelar", EVENT_OPTION_DELETE, EVENT_OPTION_DETAIL, EVENT_OPTION_EDIT_METADATA, EVENT_OPTION_MANAGE_ALBUMS).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    switch (task.Result)
                    {
                        case EVENT_OPTION_DETAIL:
                            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popup.EventDetailPopupPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel));

                            break;

                        case EVENT_OPTION_MANAGE_ALBUMS:
                            this.NavigationPushPageAsync(new ManageEventAlbumsPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel));

                            break;

                        case EVENT_OPTION_DELETE:
                            DisplayAlert("Confirmar", "Está seguro que desea eliminar este evento?", "Aceptar", "Cancelar").ContinueWith(async subTask =>
                            {
                                if (!subTask.Result) return;

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    BindingContextPageModel.IsBusy = true;
                                });

                                var client = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Managers.SessionManager.Instance);

                                System.Exception failException = null;

                                try
                                {
                                    await client.Delete(((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel).Id);
                                }
                                catch (System.Exception ex)
                                {
                                    failException = ex;
                                }

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    BindingContextPageModel.IsBusy = false;

                                    if (failException == null)
                                    {
                                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Evento eliminado con éxito.");

                                        LoadItems();
                                    }
                                    else
                                    {
                                        (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(failException);
                                    }
                                });
                            });

                            break;
                    }
                });
            });
        }

        private void AddToolbarItem_Activated(object sender, System.EventArgs e)
        {
            //if (BindingContextPageModel.IsBusy) return;

            //BindingContextPageModel.IsBusy = true;

            //System.Threading.Tasks.Task.Run(() =>
            //{
            //    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        try
            //        {
            //            string[] fileSelector = null;

            //            switch (Xamarin.Forms.Device.RuntimePlatform)
            //            {
            //                case Xamarin.Forms.Device.Android:
            //                    fileSelector = new string[] { "image/*" };

            //                    break;

            //                case Xamarin.Forms.Device.UWP:
            //                    fileSelector = new string[] { ".jpg", ".jpeg", ".png", ".bmp" };

            //                    break;

            //                case Xamarin.Forms.Device.WPF:
            //                default:
            //                    fileSelector = new string[] { "Imágenes Soportadas|*.jpg;*.jpeg;*.png;*.bmp" };

            //                    break;
            //            }

            //            var targetFileData = await Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector);

            //            if (targetFileData != null)
            //            {
            //                await this.NavigationPushPageAsync(new AddImagePage(targetFileData, OnRequiredReloadAction));
            //            }
            //        }
            //        finally
            //        {
            //            BindingContextPageModel.IsBusy = false;
            //        }
            //    });
            //});
        }

        private void OnRequiredReloadAction()
        {
            LoadItems();
        }

        private void LoadItems(int page = 0)
        {
            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var client = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Managers.SessionManager.Instance);

                return await client.GetPaginateAsync(page);
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