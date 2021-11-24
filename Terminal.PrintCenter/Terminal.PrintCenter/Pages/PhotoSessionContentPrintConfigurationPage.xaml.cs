using EM.Terminal.PrintCenter.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public partial class PhotoSessionContentPrintConfigurationPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.PhotoSessionContentPrintConfigurationPageModel>, IPhotoSessionContentPrintableViewPresenter
    {
        public PhotoSessionContentPrintConfigurationPage(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model)
            : base(new Models.Page.PhotoSessionContentPrintConfigurationPageModel
            {
                Content = model,
                PrintLayoutDescriptor = Extend.Layout.Provision.Providers.PrintLayoutDescriptorProvider.Instance.GetPrintLayoutDescriptorById(Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().TargetAlbum.DefaultPhotoSessionContentCollageLayout.Id)
            })
        {
            BindingContextPageModel.Permutation = BindingContextPageModel.PrintLayoutDescriptor.ToDefaultPermutation();

            InitializeComponent();
        }

        protected override void DisposeManagementResources()
        {
            base.DisposeManagementResources();

            (PrintLayoutIntancedContentView.Content as System.IDisposable).Dispose();

            PrintLayoutIntancedContentView.Content = null;
        }

        private void CloseButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPopModalPageAsync();
        }

        private void LessPrintCopyCountButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.PrintCopyCount <= 1) return;

            BindingContextPageModel.PrintCopyCount--;
        }

        private void MorePrintCopyCountButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.PrintCopyCount++;
        }

        private void RequestPrintButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea imprimir esta fotografía?", "Confirmar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = true;

                    var printOrdersClient = new Print.Api.Connect.Clients.PrintOrdersApiConnectClient(Xamarin.Forms.DependencyService.Get<Providers.PrintCloudHttpClientHelperProvider>());

                    var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

                    printOrdersClient.CreateAsync(new Print.Api.Common.Models.PrintOrderInputModel
                    {
                        Created = System.DateTime.Now,
                        Enabled = true,
                        Count = BindingContextPageModel.PrintCopyCount,
                        TargetContentEventLogoImageId = sessionManager.TargetEvent.Logo.Id,
                        TargetContentAlbumId = sessionManager.TargetAlbum.Id,
                        TargetContentId = BindingContextPageModel.Content.Id,
                        PermutationSerialied = Newtonsoft.Json.JsonConvert.SerializeObject(BindingContextPageModel.Permutation)
                    }).ContinueWith(printOrderCreationTask =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            if (printOrderCreationTask.Exception != null || printOrderCreationTask.IsCanceled)
                            {
                                DisplayAlert("Aviso", string.Format("No fue posible solicitar esta impresión en este momento.{0}{0}Por favor, notifique esta situación al personal de asistencia.{0}{0}Gracias y disculpe las molestias causadas.", System.Environment.NewLine), "Aceptar");
                            }
                            else
                            {
                                DisplayAlert("Aviso", string.Format("Solicitud de Impresión realizada con éxito!{0}{0}Espere unos segundos para obtener la fotografía impresa.{0}{0}Gracias!", System.Environment.NewLine), "Continuar");
                            }

                            this.NavigationPopModalPageAsync();
                        });
                    });
                });
            });
        }

        #region IPictureContentPrintableViewPresenter implementation

        public Cloud.Api.Common.Models.PhotoSessionContentDisplayModel PhotoSessionContent => BindingContextPageModel.Content;

        public int[] Permutation => BindingContextPageModel.Permutation;

        public System.Action PrintableViewPresentationFailCallbackAction => null;

        public System.Action PrintableViewPresentationReadyCallbackAction => null;

        #endregion
    }
}