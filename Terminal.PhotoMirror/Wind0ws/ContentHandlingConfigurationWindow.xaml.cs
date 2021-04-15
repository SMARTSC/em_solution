using System.Linq;

namespace EM.Terminal.PhotoMirror.Wind0ws
{
    public partial class ContentHandlingConfigurationWindow : System.Windows.Window
    {
        public ContentHandlingConfigurationWindow()
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

            if (Managers.SessionManager.Instance.HasContentHandlingConfiguration) return;

            var result = System.Windows.MessageBox.Show(string.Format("Esta configuración es obligatoria.{0}{0}La aplicación deberá cerrarse si no se completa con éxito.{0}{0}Seguro que desea cancelar?", System.Environment.NewLine), "Confirmar", System.Windows.MessageBoxButton.YesNo);

            eventArgs.Cancel = result == System.Windows.MessageBoxResult.No;

            if (eventArgs.Cancel) LoadConfigurationContext();
        }

        private void ApplyButton_Click(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            if (AlbumsComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Debe seleccionar una Album para continuar.", "Aviso");

                return;
            }

            if (CaptureDeviceComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Debe seleccionar una Cámara para continuar.", "Aviso");

                return;
            }

            Managers.SessionManager.Instance.TargetEvent = EventsComboBox.SelectedItem as Cloud.Backend.Api.Common.Models.EventDisplayModel;
            Managers.SessionManager.Instance.TargetAlbum = AlbumsComboBox.SelectedItem as Cloud.Backend.Api.Common.Models.AlbumDisplayModel;
            Managers.SessionManager.Instance.CaptureDevice = CaptureDeviceComboBox.SelectedItem as Devices.Facade.ICaptureDeviceFacade;

            DialogResult = Managers.SessionManager.Instance.HasContentHandlingConfiguration;

            Close();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            LoadConfigurationContext();
        }

        private void LoadConfigurationContext()
        {
            IsEnabled = false;

            System.Threading.Tasks.Task.Run(async () =>
            {
                var eventsApiConnectClient = new Cloud.Backend.Api.Connect.Clients.EventsApiClient(Providers.HttpClientHelperProvider.Instance);

                var result = new Models.ContentHandlingConfigurationContextModel
                {
                    // TODO Get only active events!
                    Events = await eventsApiConnectClient.GetAsync(),
                    CaptureDevices = new System.Collections.Generic.List<Devices.Facade.ICaptureDeviceFacade>()
                };

                var directShowDeviceIndex = 0;

                result.CaptureDevices.AddRange(DirectShowLib.DsDevice.GetDevicesOfCat(DirectShowLib.FilterCategory.VideoInputDevice).Select(item => new Devices.DirectShowEmguCVCaptureDevice(item, directShowDeviceIndex++)));

                result.CaptureDevices.AddRange(new Devices.Driver.Canon.CanonAPI().GetCameraList().Select(item => new Devices.CanonEDCaptureDevice(item)));

                return result;
            }).ContinueWith(task =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    IsEnabled = true;

                    if (task.Exception != null)
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

                        System.Windows.MessageBox.Show(message, "Aviso");

                        Close();
                    }
                    else
                    {
                        CaptureDeviceComboBox.Items.Clear();

                        foreach (var item in task.Result.CaptureDevices)
                        {
                            CaptureDeviceComboBox.Items.Add(item);
                        }

                        EventsComboBox.Items.Clear();

                        foreach (var item in task.Result.Events)
                        {
                            EventsComboBox.Items.Add(item);
                        }
                    }
                });
            });
        }

        private void EventsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AlbumsComboBox.Items.Clear();

            if (!(EventsComboBox.SelectedValue is Cloud.Backend.Api.Common.Models.EventDisplayModel eventDisplayModel))
            {
                AlbumsComboBox.IsEnabled = false;
            }
            else
            {
                AlbumsComboBox.IsEnabled = true;

                foreach (var item in eventDisplayModel.Albums)
                {
                    AlbumsComboBox.Items.Add(item);
                }
            }
        }
    }
}
