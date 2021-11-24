using SkiaSharp;
using System.Drawing.Printing;

namespace EM.Print.Enqueuer.StanIn.Wind0ws
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private const double BUSY_STATUS_MAX = 100;

        private Providers.PrintCloudHttpClientHelperProvider printCloudHttpClientHelperProvider;
        private Providers.ContentCloudHttpClientHelperProvider contentCloudHttpClientHelperProvider;

        private readonly System.Timers.Timer pendingPrintOrdersCheckTimer;
        private readonly System.IO.FileSystemWatcher printFilesDirectoryWatcher;

        public MainWindow()
        {
            InitializeComponent();

            BusyStatusProgressBar.Maximum = BUSY_STATUS_MAX;

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
            BusyStatusProgressBar.IsIndeterminate = true;

            StartButton.IsEnabled = false;

            var securityApiConnectClient = new Cloud.Api.Connect.Clients.SecurityApiClient(new Providers.AnonymousContentCloudHttpClientHelperProvider().HttpClientHelper);

            securityApiConnectClient.AuthenticateAsync(new Cloud.Api.Common.Models.AuthenticationIntentModel
            {
                User = ContentCloudUserLoginTextBox.Text,
                Password = ContentCloudUserPasswordTextBox.Password
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (task.Exception != null || task.IsCanceled)
                    {
                        var errorMessage = "Problemas no esperados en el sistema.";

                        if (task.Exception == null)
                        {
                            errorMessage = "El sistema ha tardado demasiado tiempo en responder.";
                        }
                        else
                        {
                            System.Exception targetException = task.Exception;

                            while (targetException is System.AggregateException)
                            {
                                targetException = targetException.InnerException;
                            }

                            if (targetException is System.Net.Http.HttpRequestException || targetException is System.Net.WebException)
                            {
                                errorMessage = "No se puede acceder al servidor. Revise su conexión de red y/o la disponibilidad del servicio.";
                            }
                            else if (targetException is SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException)
                            {
                                var unspectedHttpStatus = (targetException as SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.UnspectedHttpStatusException).HttpStatusCode;

                                if (unspectedHttpStatus == System.Net.HttpStatusCode.BadRequest)
                                {
                                    errorMessage = "Datos incorrectos.";
                                }
                                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Forbidden)
                                {
                                    errorMessage = "Usuario no autorizado.";
                                }
                                else if (unspectedHttpStatus == System.Net.HttpStatusCode.Unauthorized)
                                {
                                    errorMessage = "Usuario y/o contraseña incorrectos.";
                                }
                            }
                        }

                        System.Windows.MessageBox.Show(this, errorMessage, "Aviso", System.Windows.MessageBoxButton.OK);

                        BusyStatusProgressBar.IsIndeterminate = false;

                        StartButton.IsEnabled = true;

                        return;
                    }

                    contentCloudHttpClientHelperProvider = new Providers.ContentCloudHttpClientHelperProvider(task.Result.Type, task.Result.Token);

                    printCloudHttpClientHelperProvider = new Providers.PrintCloudHttpClientHelperProvider(PrintServerHostTextBox.Text, int.Parse(PrintServerPortTextBox.Text), PrintServerRequiredSecureConnectionCheckBox?.IsChecked ?? false);

                    printFilesDirectoryWatcher.EnableRaisingEvents = true;

                    pendingPrintOrdersCheckTimer.Start();

                    UpdateComponentState();
                });
            });
        }

        private void StopButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show(this, "Está seguro que desea detener el procesamiento de solicitudes de impresión?", "Confirmar", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.No) return;

            pendingPrintOrdersCheckTimer.Stop();

            printFilesDirectoryWatcher.EnableRaisingEvents = false;

            contentCloudHttpClientHelperProvider = null;
            printCloudHttpClientHelperProvider = null;

            UpdateComponentState();
        }

        private void PendingPrintOrdersCheckTimer_Elapsed(object sender, System.EventArgs eventArgs)
        {
            pendingPrintOrdersCheckTimer.Stop();

            System.Threading.Tasks.Task.Run(async () =>
            {
                var printOrdersClient = new Api.Connect.Clients.PrintOrdersApiConnectClient(printCloudHttpClientHelperProvider);

                if (!await printOrdersClient.StackEnabled())
                {
                    return;
                }

                var pendingPrintOrdersPagination = await printOrdersClient.FilterPaginateAsync(new Api.Common.Models.Filter.PrintOrderFilterModel { PendingOnly = true, EnabledOnly = true }, 1, int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerPrintOrdersPageFetchLimit"]));

                foreach (var printOrder in pendingPrintOrdersPagination.List)
                {
                    try
                    {
                        if (printOrder.PermutationSerialized != null)
                        {
                            var photoSessionContentApiConnectClient = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(contentCloudHttpClientHelperProvider);

                            var photoSessionContent = await photoSessionContentApiConnectClient.GetAsync(printOrder.TargetContentId);
                        }
                        else
                        {
                            var pictureContentApiConnectClient = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(contentCloudHttpClientHelperProvider);

                            var pictureContent = await pictureContentApiConnectClient.GetAsync(printOrder.TargetContentId);

                            using (var httpClient = new System.Net.Http.HttpClient())
                            {
                                var authorization = contentCloudHttpClientHelperProvider.GetAuthorization();

                                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authorization.Key, authorization.Value);

                                var pictureContentImageData = await httpClient.GetByteArrayAsync(BuildAlbumImageUri(printOrder.TargetContentAlbumId, pictureContent.Image.Id));

                                var eventLogoImageData = await httpClient.GetByteArrayAsync(BuildImageUri(printOrder.TargetContentEventLogoImageId));

                                using (var pictureContentSKBitmap = SKBitmap.Decode(pictureContentImageData))
                                {
                                    using (var eventLogoSKBitmap = SKBitmap.Decode(eventLogoImageData))
                                    {
                                        var printDocumentWidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PrintDocumentWidth"]);
                                        var printDocumentHeight = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PrintDocumentHeight"]);

                                        var printDocumentImageInfo = new SKImageInfo(printDocumentWidth, printDocumentHeight);

                                        using (var printDocumentSKBitmap = new SKBitmap(printDocumentImageInfo))
                                        {
                                            using (var printDocumentSKCanvas = new SKCanvas(printDocumentSKBitmap))
                                            {
                                                printDocumentSKCanvas.Clear(SKColors.White);

                                                var desiredPictureContentImageInfo = new SKImageInfo((int)System.Math.Round(printDocumentWidth * 0.75), (int)System.Math.Round(printDocumentHeight * 0.75));

                                                using (var pictureContentThumbnailSkBitmap = pictureContentSKBitmap.Resize(desiredPictureContentImageInfo, SKFilterQuality.High))
                                                {
                                                    var pictureContentThumbnailSKRect = SKRect.Create((int)System.Math.Round((printDocumentWidth - desiredPictureContentImageInfo.Width) * 0.5), (int)System.Math.Round(printDocumentHeight * 0.04), desiredPictureContentImageInfo.Width, desiredPictureContentImageInfo.Height);

                                                    printDocumentSKCanvas.DrawBitmap(pictureContentSKBitmap, pictureContentThumbnailSKRect);
                                                }

                                                var desiredEventLogoSide = (int)System.Math.Round(printDocumentHeight * 0.15);

                                                var desiredEventLogoImageInfo = new SKImageInfo(desiredEventLogoSide, desiredEventLogoSide);

                                                using (var eventLogoThumbnailSkBitmap = eventLogoSKBitmap.Resize(desiredEventLogoImageInfo, SKFilterQuality.High))
                                                {
                                                    var eventLogoThumbnailSKRect = SKRect.Create((int)System.Math.Round((printDocumentWidth - desiredEventLogoSide) * 0.5), (int)System.Math.Round(printDocumentHeight * 0.82), desiredEventLogoImageInfo.Width, desiredEventLogoImageInfo.Height);

                                                    printDocumentSKCanvas.DrawBitmap(eventLogoThumbnailSkBitmap, eventLogoThumbnailSKRect);
                                                }
                                            }

                                            using (var printDocumentSkImage = SKImage.FromBitmap(printDocumentSKBitmap))
                                            {
                                                using (var printDocumentSkData = printDocumentSkImage.Encode(SKEncodedImageFormat.Jpeg, 75))
                                                {
                                                    using (var fileStream = System.IO.File.Create(System.IO.Path.Combine(printFilesDirectoryWatcher.Path, string.Format("{0}_{1}.jpg", printOrder.Id, printOrder.Count))))
                                                    {
                                                        fileStream.Write(printDocumentSkData.ToArray(), 0, (int)printDocumentSkData.Size);

                                                        fileStream.Close();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                pictureContentImageData = null;

                                eventLogoImageData = null;
                            }
                        }

                        await printOrdersClient.EditAsync(new Api.Common.Models.PrintOrderInputModel
                        {
                            Id = printOrder.Id,
                            Enabled = true,
                            Attended = System.DateTime.Now
                        });
                    }
                    catch { }

                    pendingPrintOrdersCheckTimer.Start();
                }
            });
        }

        private void PrintFilesDirectoryWatcher_Created(object sender, System.IO.FileSystemEventArgs eventArgs)
        {
            //System.Threading.Tasks.Task.Run(() =>
            //{
            //    // TODO 
            //    // El fichero debe venir del Cloud con las medidas requeridas: Foto normal => 4x6 Pulgadas (10 x 15 cm) | Tiras => 2x6 Pulgadas (5 x 15 cm)

            //    var printDocument = new PrintDocument
            //    {
            //        DocumentName = eventArgs.Name
            //    };

            //    printDocument.DefaultPageSettings.Landscape = true;

            //    printDocument.PrintPage += (printPageSender, printPageEventArgs) =>
            //    {
            //        var printableImage = System.Drawing.Image.FromFile(eventArgs.FullPath);

            //        System.Drawing.PointF position = new System.Drawing.PointF(0, 0);

            //        var rectangle = printPageEventArgs.MarginBounds;

            //        rectangle.Location = new System.Drawing.Point(0, 0);

            //        rectangle.Width = printPageEventArgs.PageSettings.Bounds.Width;

            //        rectangle.Height = printPageEventArgs.PageSettings.Bounds.Height;

            //        printPageEventArgs.Graphics.DrawImage(printableImage, rectangle);
            //    };

            //    var fileNameParts = System.IO.Path.GetFileNameWithoutExtension(eventArgs.Name).Split('_');

            //    for (int i = 0; i < short.Parse(fileNameParts[1]); i++)
            //    {
            //        printDocument.Print();
            //    }
            //}).ContinueWith(task =>
            //{
            //    if (task.Exception != null || task.IsCanceled)
            //    {
            //        foreach (var exception in task.Exception.InnerExceptions)
            //        {
            //            Log("Error", string.Format("Problem has occurred when try to print this Order: {0} - {1}", eventArgs.Name, exception.Message));
            //        }

            //        return;
            //    }

            //    // TODO 
            //    // Confirmar al Cloud que la impresion se realizó correctamente.

            //    try {

            //        System.IO.File.Delete(eventArgs.FullPath);

            //    } catch {

            //        Log("Error", string.Format("Problem has occurred when try to delete this Order: {0}", eventArgs.Name));

            //    }
            //});
        }

        private void UpdateComponentState()
        {
            if (contentCloudHttpClientHelperProvider != null)
            {
                ContentCloudParamGroupBox.IsEnabled = false;
                PrintCloudParamGroupBox.IsEnabled = false;
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                BusyStatusProgressBar.Value = BUSY_STATUS_MAX;
            }
            else
            {
                ContentCloudParamGroupBox.IsEnabled = true;
                PrintCloudParamGroupBox.IsEnabled = true;
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                BusyStatusProgressBar.Value = 0;
            }

            BusyStatusProgressBar.IsIndeterminate = false;
        }

        private void Log(string level, string message)
        {
            var logDirectoryPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!System.IO.Directory.Exists(logDirectoryPath))
            {
                System.IO.Directory.CreateDirectory(logDirectoryPath);
            }

            var logFilePath = System.IO.Path.Combine(logDirectoryPath, string.Format("{0}.log", System.DateTime.Now.Date.ToShortDateString().Replace('/', '_')));

            if (!System.IO.File.Exists(logFilePath))
            { 
                using (System.IO.StreamWriter streamWriter = System.IO.File.CreateText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[PrintEnqueuerLog - {0}] - {1}: {2}", level, System.DateTime.Now.ToLongTimeString(), message));
                }
            }
            else
            {
                using (System.IO.StreamWriter streamWriter = System.IO.File.AppendText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[PrintEnqueuerLog - {0}] - {1}: {2}", level, System.DateTime.Now.ToLongTimeString(), message));
                }
            }
        }

        private string BuildAlbumImageUri(System.Guid albumId, System.Guid imageId)
        {
            return string.Format("{0}://{1}/business/media/album/{2}/image/{3}", Providers.ConstantProvider.CLOUD_CONNECTION_SCHEMA, Providers.ConstantProvider.CLOUD_SERVER_HOST, albumId, imageId);
        }

        private string BuildImageUri(System.Guid imageId)
        {
            return string.Format("{0}://{1}/business/media/image/{2}", Providers.ConstantProvider.CLOUD_CONNECTION_SCHEMA, Providers.ConstantProvider.CLOUD_SERVER_HOST, imageId);
        }
    }
}