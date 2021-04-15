namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class PictureContentReviewControlStatePresentationControl : System.Windows.Controls.UserControl
    {
        private System.Guid captureImageId;

        public PictureContentReviewControlStatePresentationControl(System.Guid captureImageId)
        {
            this.captureImageId = captureImageId;

            InitializeComponent();

            var captureImagePath = System.IO.Path.Combine(Providers.ConstantProvider.ContentCaptureDirectoryPath, string.Format("{0}.JPG", captureImageId));

            var captureBitmapImage = new System.Windows.Media.Imaging.BitmapImage();

            captureBitmapImage.BeginInit();

            captureBitmapImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
            captureBitmapImage.UriSource = new System.Uri(captureImagePath);
            captureBitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;

            captureBitmapImage.EndInit();

            CaptureImageView.Source = captureBitmapImage;
        }

        private void RepeatOptionStackPanel_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnRepeatOptionPicked();
        }

        private void RepeatOptionStackPanel_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            OnRepeatOptionPicked();
        }

        private void RepeatButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnRepeatOptionPicked();
        }

        private void CompleteOptionStackPanel_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnCompleteOptionPicked();
        }

        private void CompleteOptionStackPanel_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            OnCompleteOptionPicked();
        }

        private void CompleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnCompleteOptionPicked();
        }

        private void OnRepeatOptionPicked()
        {
            new Managers.Capture.PictureContentReCaptureManager(captureImageId).PerformNextWorkflowStep();
        }

        private void OnCompleteOptionPicked()
        {
            IsEnabled = false;
            Opacity = 0.2;

            System.Threading.Tasks.Task.Run(() =>
            {
                var contentCaptureDirectoryPath = Providers.ConstantProvider.ContentCaptureDirectoryPath;

                var targetFilePath = System.IO.Path.Combine(contentCaptureDirectoryPath, string.Format("{0}.JPG", captureImageId));

                try
                {
                    var capturedImageBackUpDirectoryPath = Providers.ConstantProvider.CapturedImageBackUpDirectoryPath;

                    if (!System.IO.Directory.Exists(capturedImageBackUpDirectoryPath))
                    {
                        System.IO.Directory.CreateDirectory(capturedImageBackUpDirectoryPath);
                    }

                    var now = System.DateTime.Now;

                    var captureImageBackUpPath = System.IO.Path.Combine(capturedImageBackUpDirectoryPath, string.Format("{0}-{1}-{2}_{3}-{4}-{5}.JPG", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second));

                    System.IO.File.Copy(targetFilePath, captureImageBackUpPath);
                }
                catch { }

                var pictureContentImageHostPath = System.IO.Path.Combine(contentCaptureDirectoryPath, string.Format("{0}_{1}_{2}.JPG", Managers.SessionManager.Instance.TargetAlbum.Id, System.Guid.Empty, captureImageId));

                System.IO.File.Move(targetFilePath, pictureContentImageHostPath);
            }).ContinueWith(task =>
            {
                if (task.Exception != null || task.IsCanceled)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        Presenters.WorkflowStatePresenter.Instance.PresentUnspectedExceptionFoundWorkflowState(task.Exception);
                    });

                    return;
                }

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri("Resources/Video/EndWorkflow.mp4", System.UriKind.Relative), Presenters.WorkflowStatePresenter.Instance.PresentHelloWorkflowState);
                });
            });
        }
    }
}
