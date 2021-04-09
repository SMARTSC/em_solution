namespace EM.Terminal.PhotoMirror.Wind0ws
{
    public partial class AuthenticationWindow : System.Windows.Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            if (!IsEnabled)
            {
                eventArgs.Cancel = true;

                return;
            }

            if (Managers.SessionManager.Instance.HasAuthenticatedSession) return;

            var result = System.Windows.MessageBox.Show(string.Format("Esta configuración es obligatoria.{0}{0}La aplicación deberá cerrarse si no se completa con éxito.{0}{0}Seguro que desea cancelar?", System.Environment.NewLine), "Confirmar", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.No || result == System.Windows.MessageBoxResult.Cancel)
            {
                eventArgs.Cancel = true;

                return;
            }
            
            eventArgs.Cancel = false;

            return;
        }

        private void ApplyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IsEnabled = false;

            var login = UserLoginTextBox.Text;
            var password = UserPasswordBox.Password;

            Managers.SessionManager.Instance.SignInAsync(login, password).ContinueWith((System.Threading.Tasks.Task task) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    IsEnabled = true;

                    if (task.Exception != null || task.IsCanceled)
                    {
                        System.Exception targetException = task.Exception;

                        while (targetException is System.AggregateException)
                        {
                            targetException = targetException.InnerException;
                        }

                        var message = "Error no esperado en el sistema.";

                        if (targetException is System.Net.Http.HttpRequestException)
                        {
                            message = string.Format("No se puede acceder al servidor.{0}{0} Revisar conexión de red y/o disponibilidad del servidor.", System.Environment.NewLine);
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

                        System.Windows.MessageBox.Show(message, "Aviso");
                    }
                    else
                    {
                        DialogResult = Managers.SessionManager.Instance.HasAuthenticatedSession;

                        Close();
                    }
                });
            });
        }
    }
}
