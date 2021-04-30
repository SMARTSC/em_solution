using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace EM.Terminal.PrintCenter.Pages.Child
{
    public partial class PrintCloudConnectChildPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.Child.PrintCloudConnectChildPageModel>
    {
        public PrintCloudConnectChildPage() : base(new Models.Page.Child.PrintCloudConnectChildPageModel())
        {
            InitializeComponent();
        }

        private void CheckButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var printCloudHttpClientHelperProvider = new Providers.PrintCloudHttpClientHelperProvider(BindingContextPageModel.Host, BindingContextPageModel.Port.Value, BindingContextPageModel.IsSecure);

            var printOrdersApiConnectClient = new Print.Api.Connect.Clients.PrintOrdersApiConnectClient(printCloudHttpClientHelperProvider);

            BindingContextPageModel.IsBusy = true;

            printOrdersApiConnectClient.StackEnabled().ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        var message = "Problemas no esperados en el sistema.";

                        if (task.Exception == null)
                        {
                            message = string.Format("El servidor de impresión ha tardado demasiado tiempo en responder.{0}{0}Es posible que la configuracion no sea correcta.", System.Environment.NewLine);
                        }
                        else
                        {
                            var targetException = task.Exception.InnerException;

                            while (targetException is System.AggregateException)
                            {
                                targetException = targetException.InnerException;
                            }

                            if (targetException is System.Net.Http.HttpRequestException)
                            {
                                message = string.Format("No se puede acceder al Servidor de Impresión especificado.{0}{0}Revise su conexión de red y/o la disponibilidad del servicio.{0}{0}Es posible que la configuracion no sea correcta.", System.Environment.NewLine);
                            }
                            else if (targetException is SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException)
                            {
                                var unspectedHttpStatus = (targetException as SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException).HttpStatusCode;

                                if (unspectedHttpStatus == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    message = string.Format("Esta versión del Centro de Impresión no es compatible con el Servidor de Impresión especificado.{0}{0}Debe actualizar la aplicación o especificar un Servidor de Impresión compatible.", System.Environment.NewLine); ;
                                }
                            }
                        }

                        DisplayAlert("Aviso", message, "Aceptar");

                        BindingContextPageModel.IsBusy = false;

                        return;
                    }

                    Xamarin.Forms.DependencyService.RegisterSingleton(printCloudHttpClientHelperProvider);

                    this.NavigationPopModalPageAsync();
                });
            });
        }
    }
}