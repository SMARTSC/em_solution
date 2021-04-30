using System.Drawing.Printing;

namespace EM.Print.Enqueuer.StanIn.Wind0ws
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Providers.PrintCloudHttpClientHelperProvider printCloudHttpClientHelperProvider;

        private readonly System.Timers.Timer pendingPrintOrdersCheckTimer;
        private readonly System.IO.FileSystemWatcher printFilesDirectoryWatcher;

        public MainWindow()
        {
            InitializeComponent();

            var printFilesDirectoryWatcherPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Orders");

            if (!System.IO.Directory.Exists(printFilesDirectoryWatcherPath))
            {
                System.IO.Directory.CreateDirectory(printFilesDirectoryWatcherPath);
            }

            pendingPrintOrdersCheckTimer = new System.Timers.Timer(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerPrintOrdersPageFetchInterval"]))
            {
                AutoReset = true
            };

            pendingPrintOrdersCheckTimer.BeginInit();

            pendingPrintOrdersCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(PendingPrintOrdersCheckTimer_Elapsed);

            pendingPrintOrdersCheckTimer.EndInit();

            printFilesDirectoryWatcher = new System.IO.FileSystemWatcher
            {
                EnableRaisingEvents = false,
                Path = printFilesDirectoryWatcherPath
            };

            printFilesDirectoryWatcher.BeginInit();
            
            printFilesDirectoryWatcher.Created += new System.IO.FileSystemEventHandler(PrintFilesDirectoryWatcher_Created);

            printFilesDirectoryWatcher.EndInit();

            UpdateComponentState();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            eventArgs.Cancel = System.Windows.MessageBox.Show(this, "Esta seguro que desea cerrar la aplicación?", "Confirmar", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No;
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            printCloudHttpClientHelperProvider = new Providers.PrintCloudHttpClientHelperProvider(PrintServerHostTextBox.Text, int.Parse(PrintServerPortText.Text), PrintServerRequiredSecureConnectionCheckBox?.IsChecked ?? false);

            printFilesDirectoryWatcher.EnableRaisingEvents = true;

            pendingPrintOrdersCheckTimer.Start();

            UpdateComponentState();
        }

        private void StopButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show(this, "Está seguro que desea detener el procesamiento de solicitudes de impresión?", "Confirmar", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.No) return;

            pendingPrintOrdersCheckTimer.Stop();

            printFilesDirectoryWatcher.EnableRaisingEvents = false;

            printCloudHttpClientHelperProvider = null;

            UpdateComponentState();
        }

        private void PendingPrintOrdersCheckTimer_Elapsed(object sender, System.EventArgs eventArgs)
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var printOrdersClient = new Api.Connect.Clients.PrintOrdersApiConnectClient(printCloudHttpClientHelperProvider);

                if (!await printOrdersClient.StackEnabled())
                {
                    // No existen ordenes de impresion sin atender en el Print Cloud.

                    return;
                }

                var pendingPrintOrdersPagination = await printOrdersClient.FilterPaginateAsync(new Api.Common.Models.Filter.PrintOrderFilterModel { PendingOnly = true, EnabledOnly = true }, 1, int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerPrintOrdersPageFetchLimit"]));

                foreach (var printOrder in pendingPrintOrdersPagination.List)
                {
                    using (var fileStream = System.IO.File.Create(System.IO.Path.Combine(printFilesDirectoryWatcher.Path, string.Format("{0}_{1}.jpg", printOrder.Id, printOrder.Count))))
                    {
                        fileStream.Write(printOrder.Data, 0, printOrder.Data.Length);

                        fileStream.Close();
                    }

                    await printOrdersClient.EditAsync(new Api.Common.Models.PrintOrderInputModel
                    {
                        Id = printOrder.Id,
                        Enabled = true,
                        Attended = System.DateTime.Now,
                        Data = new byte[0]
                    });
                }
            });
        }

        private void PrintFilesDirectoryWatcher_Created(object sender, System.IO.FileSystemEventArgs eventArgs)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                // TODO el fichero debe venir del CLOUD con las medidas requeridas: Foto normal => 4x6 Pulgadas (10 x 15 cm) | Tiras => 2x6 Pulgadas (5 x 15 cm)

                var printDocument = new PrintDocument
                {
                    DocumentName = eventArgs.Name,
                    PrintController = new StandardPrintController()
                };

                printDocument.DefaultPageSettings.Landscape = true;

                printDocument.PrintPage += (printPageSender, printPageEventArgs) =>
                {
                    var printableImage = System.Drawing.Image.FromFile(eventArgs.FullPath);

                    System.Drawing.PointF position = new System.Drawing.PointF(0, 0);

                    var rectangle = printPageEventArgs.MarginBounds;

                    rectangle.Location = new System.Drawing.Point(0, 0);

                    rectangle.Width = printPageEventArgs.PageSettings.Bounds.Width;
                    rectangle.Height = printPageEventArgs.PageSettings.Bounds.Height;

                    printPageEventArgs.Graphics.DrawImage(printableImage, rectangle);
                };

                var fileNameParts = System.IO.Path.GetFileNameWithoutExtension(eventArgs.Name).Split('_');

                for (int i = 0; i < short.Parse(fileNameParts[1]); i++)
                {
                    printDocument.Print();
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null || task.IsCanceled)
                {
                    return;
                }

                // TODO Confirmar al Cloud que la impresion se realizó correctamente, enviando el ID del Contenido.

                try { System.IO.File.Delete(eventArgs.FullPath); } catch { }
            });
        }

        private void UpdateComponentState()
        {
            if (printCloudHttpClientHelperProvider != null)
            {
                PrintServerHostTextBox.IsEnabled = false;
                PrintServerPortText.IsEnabled = false;
                PrintServerRequiredSecureConnectionCheckBox.IsEnabled = false;
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
            }
            else
            {
                PrintServerHostTextBox.IsEnabled = true;
                PrintServerPortText.IsEnabled = true;
                PrintServerRequiredSecureConnectionCheckBox.IsEnabled = true;
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
            }
        }
    }
}