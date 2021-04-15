namespace EM.Terminal.PrintCenter
{
    public partial class App : Xamarin.Forms.Application, Common.Helpers.Facade.IExceptionHandleHelperFacade
    {
        private const string SUPPORT_ORIENTATION_MESSAGE = "Por favor contacte con el servicio de asistencia técnica para notificar esta situación.";

        public App()
        {
            InitializeComponent();

            Xamarin.Forms.DependencyService.RegisterSingleton(new Managers.SessionManager());
            Xamarin.Forms.DependencyService.RegisterSingleton(new Providers.HttpClientHelperProvider());
            Xamarin.Forms.DependencyService.RegisterSingleton(new Providers.ContentPaginationProvider());
            Xamarin.Forms.DependencyService.RegisterSingleton<Common.Helpers.Facade.IExceptionHandleHelperFacade>(this);

            FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                HttpClient = new System.Net.Http.HttpClient(new Helpers.HttpClientHandlingHelper()),
                HttpHeadersTimeout = 60000,
                HttpReadTimeout = 90000
            });

            InitPresentationWorkflow();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void InitPresentationWorkflow()
        {
            MainPage = new Pages.StartUpPage();
        }

        private void RestartPresentationWorkflow()
        {
            Xamarin.Forms.DependencyService.Get<Managers.SessionManager>().SignOut();

            InitPresentationWorkflow();
        }

        #region Helpers.Facade.IExceptionHandleHelperFacade implementation
        public void HandleException(System.Exception exception, System.Action continuationCallback = null)
        {
            if (exception == null)
            {
                MainPage.DisplayAlert("Aviso", string.Format("El sistema ha tardado demasiado tiempo en responder.{1}{1}{0}", SUPPORT_ORIENTATION_MESSAGE, System.Environment.NewLine), "Aceptar").ContinueWith(_ => { continuationCallback?.Invoke(); });

                return;
            }

            System.Exception targetException = exception;

            while (targetException is System.AggregateException)
            {
                targetException = targetException.InnerException;
            }

            var message = "Problemas no esperados en el sistema.";

            if (targetException is System.Net.Http.HttpRequestException)
            {
                message = "No se puede acceder al servidor. Revise su conexión de red y/o la disponibilidad del servicio.";
            }
            else if (targetException is SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException)
            {
                var unspectedHttpStatus = (targetException as SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException).HttpStatusCode;

                if (unspectedHttpStatus == System.Net.HttpStatusCode.BadRequest)
                {
                    message = "Datos del envío incorrectos.";
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    message = "Acceso denegado.";

                    continuationCallback = () =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(RestartPresentationWorkflow);
                    };
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.PreconditionFailed)
                {
                    message = "Es obligatorio que cambie su contraseña para continuar.";

                    continuationCallback = () =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(RestartPresentationWorkflow);
                    };
                }
                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    message = "No autorizado.";

                    continuationCallback = () =>
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(RestartPresentationWorkflow);
                    };
                }
            }

            MainPage.DisplayAlert("Error", message, "Aceptar").ContinueWith(task => { continuationCallback?.Invoke(); });
        }
        #endregion
    }
}
