using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.Dashboard.Pages
{
    public partial class ManageEventAlbumsPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageEventAlbumsPageModel>
    {
        private const string ALBUM_OPTION_DETAIL = "Detalles";
        private const string ALBUM_OPTION_EDIT_METADATA = "Modificar Metadatos";
        private const string ALBUM_OPTION_MANAGE_PHOTO_CONTENTS = "Gestionar Contenidos (Retratos)";
        private const string ALBUM_OPTION_MANAGE_PHOTOSESSION_CONTENTS = "Gestionar Contenidos (Sesión de Foto)";
        private const string ALBUM_OPTION_MANAGE_VIDEO_CONTENTS = "Gestionar Contenidos (Video)";
        private const string ALBUM_OPTION_DELETE = "Eliminar";

        public ManageEventAlbumsPage(Cloud.Backend.Api.Common.Models.EventDisplayModel @event) 
            : base(new Models.Page.ManageEventAlbumsPageModel { IsBusy = true, Event = @event })
        {
            InitializeComponent();
        }

        private void AlbumOptionsButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayActionSheet("Opciones de Album", "Cancelar", ALBUM_OPTION_DELETE, ALBUM_OPTION_DETAIL, ALBUM_OPTION_EDIT_METADATA, ALBUM_OPTION_MANAGE_PHOTO_CONTENTS, ALBUM_OPTION_MANAGE_PHOTOSESSION_CONTENTS).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    switch (task.Result)
                    {
                        case ALBUM_OPTION_MANAGE_PHOTO_CONTENTS:
                            this.NavigationPushPageAsync(new ManageEventAlbumPictureContentsPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.AlbumDisplayModel));

                            break;

                        case ALBUM_OPTION_MANAGE_PHOTOSESSION_CONTENTS:
                            this.NavigationPushPageAsync(new ManageEventAlbumPhotoSessionContentsPage((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.AlbumDisplayModel));

                            break;

                        case ALBUM_OPTION_DELETE:
                            //DisplayAlert("Confirmar", "Está seguro que desea eliminar este album?", "Aceptar", "Cancelar").ContinueWith(async subTask =>
                            //{
                            //    if (!subTask.Result) return;

                            //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            //    {
                            //        BindingContextPageModel.IsBusy = true;
                            //    });

                            //    var client = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Managers.SessionManager.Instance);

                            //    System.Exception failException = null;

                            //    try
                            //    {
                            //        await client.Delete(((sender as Xamarin.Forms.BindableObject).BindingContext as Cloud.Backend.Api.Common.Models.EventDisplayModel).Id);
                            //    }
                            //    catch (System.Exception ex)
                            //    {
                            //        failException = ex;
                            //    }

                            //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            //    {
                            //        BindingContextPageModel.IsBusy = false;

                            //        if (failException == null)
                            //        {
                            //            Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Evento eliminado con éxito.");

                            //            LoadItems();
                            //        }
                            //        else
                            //        {
                            //            (Xamarin.Forms.Application.Current as Helpers.Facade.IExceptionHandleHelperFacade).HandleException(failException);
                            //        }
                            //    });
                            //});

                            break;
                    }
                });
            });
        }
    }
}