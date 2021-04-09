namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class PhotoSessionContentReviewStatePresentationControl : System.Windows.Controls.UserControl
    {
        private readonly System.Guid[] captureImageIds;

        private int targetCaptureImageIndex;

        public PhotoSessionContentReviewStatePresentationControl(System.Guid[] captureImageIds)
        {
            this.captureImageIds = captureImageIds;

            InitializeComponent();

            var captureImageIdsLength = captureImageIds.Length;

            for (int i = 0; i < captureImageIdsLength; i++)
            {
                var captureImagePath = System.IO.Path.Combine(Providers.ConstantProvider.ContentCaptureDirectoryPath, string.Format("{0}.JPG", captureImageIds[i]));

                var captureBitmapImage = new System.Windows.Media.Imaging.BitmapImage();

                captureBitmapImage.BeginInit();

                captureBitmapImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                captureBitmapImage.UriSource = new System.Uri(captureImagePath);
                captureBitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;

                captureBitmapImage.EndInit();

                switch (i)
                {
                    case 0:
                        FirstCaptureImageThumbnailView.Source = captureBitmapImage;

                        break;

                    case 1:
                        SecoundCaptureImageThumbnailView.Source = captureBitmapImage;

                        break;

                    case 2:
                    default:
                        ThirthCaptureImageThumbnailView.Source = captureBitmapImage;

                        break;
                }

                if (TargetCaptureImageView.Source != null) continue;

                TargetCaptureImageView.Source = captureBitmapImage;

                targetCaptureImageIndex = 0;
            }

            PreviousButton.Visibility = System.Windows.Visibility.Hidden;
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

        private void PreviousButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (targetCaptureImageIndex <= 0) return;

            targetCaptureImageIndex--;

            if (targetCaptureImageIndex == 1)
            {
                TargetCaptureImageView.Source = SecoundCaptureImageThumbnailView.Source;

                NextButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                TargetCaptureImageView.Source = FirstCaptureImageThumbnailView.Source;

                PreviousButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void NextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (targetCaptureImageIndex >= captureImageIds.Length - 1) return;

            targetCaptureImageIndex++;

            if (targetCaptureImageIndex == 1)
            {
                TargetCaptureImageView.Source = SecoundCaptureImageThumbnailView.Source;

                PreviousButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                TargetCaptureImageView.Source = ThirthCaptureImageThumbnailView.Source;

                NextButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void OnRepeatOptionPicked()
        {
            new Managers.Capture.PhotoSessionContentReCaptureManager(captureImageIds).PerformNextWorkflowStep();
        }

        private void OnCompleteOptionPicked()
        {
            IsEnabled = false;
            Opacity = 0.2;

            var photoSessionContentsApiConnectClient = new Cloud.Backend.Api.Connect.Clients.PhotoSessionContentsApiClient(Providers.HttpClientHelperProvider.Instance);

            var photoSessionContent = new Cloud.Backend.Api.Common.Models.PhotoSessionContentInputModel
            {
                Id = System.Guid.NewGuid(),
                AlbumId = Managers.SessionManager.Instance.TargetAlbum.Id,
                Created = System.DateTime.Now,
                Enabled = true
            };

            photoSessionContentsApiConnectClient.CreateAsync(photoSessionContent).ContinueWith(createContentTask =>
            {
                if (createContentTask.Exception != null || createContentTask.IsCanceled)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        Presenters.WorkflowStatePresenter.Instance.PresentUnspectedExceptionFoundWorkflowState(createContentTask.Exception);
                    });

                    return;
                }

                var index = 0;

                var capturedImageBackUpDirectoryPath = Providers.ConstantProvider.CapturedImageBackUpDirectoryPath;

                if (!System.IO.Directory.Exists(capturedImageBackUpDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(capturedImageBackUpDirectoryPath);
                }

                foreach (var captureImageId in captureImageIds)
                {
                    var contentCaptureDirectoryPath = Providers.ConstantProvider.ContentCaptureDirectoryPath;

                    var pictureContentImageHostPath = System.IO.Path.Combine(contentCaptureDirectoryPath, string.Format("{0}_{1}_{2}.JPG", Managers.SessionManager.Instance.TargetAlbum.Id, photoSessionContent.Id, captureImageId));

                    System.IO.File.Move(System.IO.Path.Combine(contentCaptureDirectoryPath, string.Format("{0}.JPG", captureImageId)), pictureContentImageHostPath);

                    try
                    {
                        var now = System.DateTime.Now;

                        var captureImageBackUpPath = System.IO.Path.Combine(capturedImageBackUpDirectoryPath, string.Format("{0}-{1}-{2}_{3}-{4}-{5}_{6}.JPG", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, index + 1));

                        System.IO.File.Copy(pictureContentImageHostPath, captureImageBackUpPath);
                    }
                    catch { }

                    index++;
                }
            }).ContinueWith(uploadsTask =>
            {
                if (uploadsTask.Exception != null || uploadsTask.IsCanceled)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        Presenters.WorkflowStatePresenter.Instance.PresentUnspectedExceptionFoundWorkflowState(uploadsTask.Exception);
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