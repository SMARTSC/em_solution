using System.Windows.Media.Animation;

namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class CameraOuturatorSimulationStatePresentationControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        private Managers.Capture.Facade.IContentCaptureManagerFacade contentCaptureManager;

        public CameraOuturatorSimulationStatePresentationControl(Managers.Capture.Facade.IContentCaptureManagerFacade contentCaptureManager)
        {
            this.contentCaptureManager = contentCaptureManager;

            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs eventArgs)
        {
            var storyboard = new Storyboard();

            var flashAnimation = new DoubleAnimation()
            {
                To = 1,
                Duration = System.TimeSpan.FromMilliseconds(100),
                EasingFunction = new QuarticEase { },
                AutoReverse = true
            };

            Storyboard.SetTarget(flashAnimation, FlashCanvas);
            Storyboard.SetTargetProperty(flashAnimation, new System.Windows.PropertyPath(OpacityProperty));

            storyboard.Children.Add(flashAnimation);

            storyboard.Completed += FlashAnimation_Complete;

            storyboard.Begin(this);
        }

        private void FlashAnimation_Complete(object sender, System.EventArgs eventArgs)
        {
            Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri(string.Format("Resources/Video/CameraCaptureComplete-{0}.mp4", new System.Random().Next(1, 3)), System.UriKind.Relative), contentCaptureManager.PerformNextWorkflowStep);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                contentCaptureManager = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
