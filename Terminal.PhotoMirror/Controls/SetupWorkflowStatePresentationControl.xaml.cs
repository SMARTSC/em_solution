namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class SetupWorkflowStatePresentationControl : System.Windows.Controls.UserControl
    {
        public SetupWorkflowStatePresentationControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            if (!Managers.SessionManager.Instance.HasAuthenticatedSession)
            {
                var authenticationWindow = new Wind0ws.AuthenticationWindow();

                var authenticationResult = authenticationWindow.ShowDialog();

                if (authenticationResult != true)
                {
                    System.Windows.Application.Current.MainWindow.Close();

                    return;
                }

                else if (!Managers.SessionManager.Instance.HasContentHandlingConfiguration)
                {
                    var contentHandlingConfigurationWindow = new Wind0ws.ContentHandlingConfigurationWindow();

                    var contentHandlingConfigurationResult = contentHandlingConfigurationWindow.ShowDialog();

                    if (contentHandlingConfigurationResult != true) System.Windows.Application.Current.MainWindow.Close();
                }

                var enterConfigurationSecurityCodeWindow = new Wind0ws.EnterConfigurationSecurityCodeWindow(true);

                enterConfigurationSecurityCodeWindow.ShowDialog();
            }

            System.Threading.Tasks.Task.Run(Managers.ContentUploadServiceManager.Instance.StartService).ContinueWith(task =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        System.Windows.MessageBox.Show("El servicio de carga de contenidos al servidor no está disponible.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                        System.Windows.Application.Current.MainWindow.Close();

                        return;
                    }

                    Presenters.WorkflowStatePresenter.Instance.PresentHelloWorkflowState();
                });
            });
        }
    }
}
