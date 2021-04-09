namespace EM.Terminal.PhotoMirror.Uploader
{
    public partial class CaptureContentUploadService : System.ServiceProcess.ServiceBase, SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private string apiConnectAuthenticationType;
        private string apiConnectAuthenticationToken;

        public CaptureContentUploadService()
        {
            InitializeComponent();

            ServiceName = Common.Providers.CommonConstantProvider.UPLOAD_SERVICE_NAME;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                apiConnectAuthenticationType = args[0];
                apiConnectAuthenticationToken = args[1];

                ContentCaptureDirectorySystemWatcher.Path = args[2];

                ContentCaptureDirectorySystemWatcher.EnableRaisingEvents = true;
            }
            catch (System.Exception exception)
            {
                using (var eventLog = new System.Diagnostics.EventLog
                {
                    Source = ServiceName
                })
                {
                    eventLog.WriteEntry(string.Format("Starting fail: {0}", exception.Message), System.Diagnostics.EventLogEntryType.Error);
                }

                Log("Trace", exception.StackTrace);

                throw exception;
            }
        }

        protected override void OnStop()
        {
            ContentCaptureDirectorySystemWatcher.EnableRaisingEvents = false;

            ContentCaptureDirectorySystemWatcher.Dispose();
        }

        private void ContentCaptureDirectorySystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs eventArgs)
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                var metadataName = eventArgs.Name.Split('.')[0];

                var parceableGuids = metadataName.Split('_');

                var targetAlbumId = System.Guid.Parse(parceableGuids[0]);
                var photoSessionContentId = System.Guid.Parse(parceableGuids[1]);
                var pictureContentId = System.Guid.Parse(parceableGuids[2]);

                if (photoSessionContentId == System.Guid.Empty)
                {
                    var pictureContentsApiConnectClient = new Cloud.Backend.Api.Connect.Clients.PictureContentsApiClient(this);

                    var inputModel = new Cloud.Backend.Api.Common.Models.PictureContentInputModel
                    {
                        Id = pictureContentId,
                        Enabled = true,
                        Created = System.IO.File.GetCreationTime(eventArgs.FullPath),
                        ContentType = "image/jpeg",
                        AlbumId = targetAlbumId
                    };

                    var readed = false;

                    while (!readed)
                    {
                        try
                        {
                            using (var captureImageFileStream = System.IO.File.OpenRead(eventArgs.FullPath))
                            {
                                inputModel.ContentData = new byte[captureImageFileStream.Length];

                                captureImageFileStream.Read(inputModel.ContentData, 0, (int)captureImageFileStream.Length);
                            }

                            readed = true;
                        }
                        catch (System.Exception exception)
                        {
                            using (var eventLog = new System.Diagnostics.EventLog
                            {
                                Source = ServiceName
                            })
                            {
                                eventLog.WriteEntry(string.Format("Reading file fail: {0}", exception.Message), System.Diagnostics.EventLogEntryType.Error);
                            }

                            Log("Trace", exception.StackTrace);

                            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));
                        }
                    }

                    await pictureContentsApiConnectClient.CreateAsync(inputModel);
                }
                else
                {
                    var photoSessionContentsApiConnectClient = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(this);

                    var inputModel = new Cloud.Backend.Api.Common.Models.ImageMediaInputModel
                    {
                        ContentType = "image/jpeg"
                    };

                    var readed = false;

                    while (!readed)
                    {
                        try
                        {
                            using (var captureImageFileStream = System.IO.File.OpenRead(eventArgs.FullPath))
                            {
                                inputModel.ContentData = new byte[captureImageFileStream.Length];

                                captureImageFileStream.Read(inputModel.ContentData, 0, (int)captureImageFileStream.Length);
                            }

                            readed = true;
                        }
                        catch (System.Exception exception)
                        {
                            using (var eventLog = new System.Diagnostics.EventLog
                            {
                                Source = ServiceName
                            })
                            {
                                eventLog.WriteEntry(string.Format("Reading file fail: {0}", exception.Message), System.Diagnostics.EventLogEntryType.Error);
                            }

                            Log("Trace", exception.StackTrace);

                            System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));
                        }
                    }

                    await photoSessionContentsApiConnectClient.AgregateAsync(photoSessionContentId, inputModel);
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null || task.IsCanceled)
                {
                    using (var eventLog = new System.Diagnostics.EventLog
                    {
                        Source = ServiceName
                    })
                    {
                        foreach (var exception in task.Exception.InnerExceptions)
                        {
                            eventLog.WriteEntry(string.Format("Upload file fail: {0}", exception?.Message ?? "Task excecution timeout!"), System.Diagnostics.EventLogEntryType.Error);

                            if (exception == null) continue;

                            Log("Trace", exception.StackTrace);
                        }
                    }
                }
                else
                {
                    System.IO.File.Delete(eventArgs.FullPath);                    
                }
            });
        }

        private System.Collections.Generic.KeyValuePair<string, string> AuthorizeHeader
        {
            get
            {
                return new System.Collections.Generic.KeyValuePair<string, string>(apiConnectAuthenticationType, apiConnectAuthenticationToken);
            }
        }

        public void Log(string level, string message)
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
                    streamWriter.WriteLine(string.Format("[{0}]{1}: {2}", level, System.DateTime.Now.ToShortTimeString(), message));
                }
            }
            else
            {
                using (System.IO.StreamWriter streamWriter = System.IO.File.AppendText(logFilePath))
                {
                    streamWriter.WriteLine(string.Format("[{0}]{1}: {2}", level, System.DateTime.Now.ToShortTimeString(), message));
                }
            }
        }

        #region IHttpClientHelperProviderFacade implementation

        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("{0}://{1}/api/", Common.Providers.CommonConstantProvider.CLOUD_CONNECTION_SCHEMA, Common.Providers.CommonConstantProvider.CLOUD_SERVER_HOST), () => AuthorizeHeader, null);

        #endregion
    }
}
