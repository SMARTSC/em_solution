namespace EM.Terminal.PrintCenter.Pages.Child
{
    public partial class UserAuthenticationChildPage : SmartSolucionesCuba.XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.Child.UserAuthenticationChildPageModel>
    {
        public event System.EventHandler Authenticated;

        public UserAuthenticationChildPage() : base(new Models.Page.Child.UserAuthenticationChildPageModel())
        {
            InitializeComponent();
        }

        private void SignButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.IsBusy = true;

            var sessionManager = Xamarin.Forms.DependencyService.Get<Managers.SessionManager>();

            sessionManager.SignInAsync(BindingContextPageModel.Login, BindingContextPageModel.Password).ContinueWith(task =>
            {
                if (task.Exception != null || task.IsCanceled)
                {
                    if (task.Exception == null)
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            BindingContextPageModel.IsBusy = false;

                            Xamarin.Forms.DependencyService.Get<Common.Helpers.Facade.IExceptionHandleHelperFacade>().HandleException(task.Exception);
                        });

                        return;
                    }

                    System.Exception targetException = task.Exception;

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
                            message = "Datos incorrectos.";
                        }
                        else if (unspectedHttpStatus == System.Net.HttpStatusCode.Forbidden)
                        {
                            message = "Usuario no autorizado.";
                        }
                        else if (unspectedHttpStatus == System.Net.HttpStatusCode.Unauthorized)
                        {
                            message = "Usuario y/o contraseña incorrectos.";
                        }
                    }

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        BindingContextPageModel.IsBusy = false;

                        DisplayAlert("Error", message, "Aceptar");
                    });

                    return;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    Authenticated?.Invoke(this, System.EventArgs.Empty);
                });
            });            
        }
    }
}