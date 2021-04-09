namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class ContentCreationModePickStatePresentationControl : System.Windows.Controls.UserControl
    {
        public ContentCreationModePickStatePresentationControl()
        {
            InitializeComponent();
        }

        private void PhotoSessionOptionRectanle_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnPhotoSessionOptionPicked();
        }

        private void PhotoSessionOptionRectangle_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            OnPhotoSessionOptionPicked();
        }

        private void PictureOptionRectangle_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            OnPictureOptionPicked();
        }

        private void PictureOptionRectangle_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnPictureOptionPicked();
        }

        private void OnPhotoSessionOptionPicked()
        {
            new Managers.Capture.PhotoSessionContentCaptureManager().PerformNextWorkflowStep();
        }

        private void OnPictureOptionPicked()
        {
            new Managers.Capture.PictureContentCaptureManager().PerformNextWorkflowStep();
        }
    }
}
