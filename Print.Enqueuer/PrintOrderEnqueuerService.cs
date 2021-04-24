using System.Drawing.Printing;

namespace EM.Print.Enqueuer
{
    public partial class PrintOrderEnqueuerService : System.ServiceProcess.ServiceBase
    {
        private readonly Providers.PrintCloudHttpClientHelperProvider printCloudHttpClientHelperProvider;

        private System.Timers.Timer pendingPrintOrdersCheckTimer;

        public PrintOrderEnqueuerService()
        {
            InitializeComponent();

            printCloudHttpClientHelperProvider = new Providers.PrintCloudHttpClientHelperProvider();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var printFilesDirectoryWatcherPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Orders");

                if (!System.IO.Directory.Exists(printFilesDirectoryWatcherPath))
                {
                    System.IO.Directory.CreateDirectory(printFilesDirectoryWatcherPath);
                }

                pendingPrintOrdersCheckTimer = new System.Timers.Timer(int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerPrintOrdersPageFetchInterval"]))
                {
                    AutoReset = true
                };

                pendingPrintOrdersCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(PendingPrintOrdersCheckTimer_Elapsed);

                PrintFilesDirectoryWatcher.Path = printFilesDirectoryWatcherPath;
                PrintFilesDirectoryWatcher.EnableRaisingEvents = true;

                pendingPrintOrdersCheckTimer.Start();

            }
            catch (System.Exception exception)
            {
                Log("Trace", "---------- Starting exceptions found ----------");

                Log("PrintService - Error", exception.Message + "Trace >>>> " + exception.StackTrace);

                throw exception;
            }
        }

        protected override void OnStop()
        {
            pendingPrintOrdersCheckTimer.Stop();

            pendingPrintOrdersCheckTimer = null;

            PrintFilesDirectoryWatcher.EnableRaisingEvents = false;

            PrintFilesDirectoryWatcher.Dispose();
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
                    using (var fileStream = System.IO.File.Create(System.IO.Path.Combine(PrintFilesDirectoryWatcher.Path, string.Format("{0}_{1}.jpg", printOrder.Id, printOrder.Count))))
                    {
                        fileStream.Write(printOrder.Data, 0, printOrder.Data.Length);

                        fileStream.Close();
                    }

                    await printOrdersClient.EditAsync(new Api.Common.Models.PrintOrderInputModel
                    {
                        Id = printOrder.Id,
                        Enabled = true,
                        Attended = System.DateTime.Now
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
                    DocumentName = eventArgs.Name
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
                    Log("Trace", "---------- Printing exceptions found ----------");

                    foreach (var exception in task.Exception.InnerExceptions)
                    {
                        Log("Error", exception.Message);
                    }

                    return;
                }

                // TODO Confirmar al Cloud que la impresion se realizó correctamente, enviando el ID del Contenido.

                try { System.IO.File.Delete(eventArgs.FullPath); } catch { }
            });
        }

        private void Log(string level, string message)
        {
            var logDirecotryPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!System.IO.Directory.Exists(logDirecotryPath))
            {
                System.IO.Directory.CreateDirectory(logDirecotryPath);
            }

            var logFilePath = System.IO.Path.Combine(logDirecotryPath, string.Format("{0}.log", System.DateTime.Now.Date.ToShortDateString().Replace('/', '_')));

            if (!System.IO.File.Exists(logFilePath))
            {
                // Create a file to write to.   
                using (System.IO.StreamWriter streamWriter = System.IO.File.CreateText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[PrintEnqueuerLog - {0}] - {1}: {2}", level, System.DateTime.Now.ToShortTimeString(), message));
                }
            }
            else
            {
                using (System.IO.StreamWriter streamWriter = System.IO.File.AppendText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[PrintEnqueuerLog - {0}] - {1}: {2}", level, System.DateTime.Now.ToShortTimeString(), message));
                }
            }
        }
    }
}