using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages
{
    public class PhotoSessionContentPrintPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.ContentPage
    {
        private readonly int printCount;

        private Views.Layout.Print.BasePhotoSessionContentPrintLayoutView printableView;

        public PhotoSessionContentPrintPage(Cloud.Api.Common.Models.PhotoSessionContentDisplayModel model, int[] permutation, int printCount, Extend.Layout.Descriptors.IPrintLayoutDescriptor printLayoutDescriptor)
        {
            this.printCount = printCount;

            printableView = Xamarin.Forms.DependencyService.Get<Providers.PrintLayoutViewProvider>().GetPhotoSessionContentPrintLayoutView(model, permutation, false, printLayoutDescriptor, PresentationFail_Callback, PresentationReady_Callback);
            printableView.WidthRequest = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height * 3 / 2;

            printableView.HorizontalOptions = Xamarin.Forms.LayoutOptions.Center;

            Content = new Xamarin.Forms.Grid
            {
                BackgroundColor = Xamarin.Forms.Color.Black,
                Children = {
                    printableView
                }
            };
        }

        private void PresentationReady_Callback()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Xamarin.Forms.DependencyService.Get<Providers.Facade.IRenderizationImageProviderFacade>().GetRenderizationImageDataAsync(printableView).ContinueWith(renderizationImageDataTask =>
                {
                    var printOrdersClient = new Print.Api.Connect.Clients.PrintOrdersApiConnectClient(Xamarin.Forms.DependencyService.Get<Providers.PrintCloudHttpClientHelperProvider>());

                    printOrdersClient.CreateAsync(new Print.Api.Common.Models.PrintOrderInputModel
                    {
                        Created = System.DateTime.Now,
                        Count = printCount,
                        Data = renderizationImageDataTask.Result,
                        Enabled = true
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

        private void PresentationFail_Callback()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Aviso", string.Format("No se puede acceder al recurso solicitado en este momento.{0}{0}Por favor, notifique esta situación al personal de asistencia.{0}{0}Gracias y disculpe las molestias causadas.", System.Environment.NewLine), "Aceptar");

                this.NavigationPopModalPageAsync();
            });
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            (Content as Xamarin.Forms.Grid).Children.RemoveAt(0);

            printableView.Dispose();

            printableView = null;
        }
    }
}