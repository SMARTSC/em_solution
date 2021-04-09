namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class HelloWorkflowStatePresentationControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        public HelloWorkflowStatePresentationControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AnimationMediaElement.Play();
        }

        private void AnimationMediaElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            AnimationMediaElement.Stop();
            AnimationMediaElement.Play();
        }

        private void AnimationMediaElement_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            OnAnimationMediaElementInteraction();
        }

        private void AnimationMediaElement_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnAnimationMediaElementInteraction();
        }

        private void ConfigurationPackIcon_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            OnConfigurationPackIconInteraction();
        }

        private void ConfigurationPackIcon_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnConfigurationPackIconInteraction();
        }

        private void OnAnimationMediaElementInteraction()
        {
            Presenters.WorkflowStatePresenter.Instance.PresentContentCreationModePickWorkflowState();
        }

        private void OnConfigurationPackIconInteraction()
        {
            if (Managers.SessionManager.Instance.ConfigurationSecurityCode != null)
            {
                var enterConfigurationSecurityCodeWindow = new Wind0ws.EnterConfigurationSecurityCodeWindow();

                var result = enterConfigurationSecurityCodeWindow.ShowDialog();

                if (result != null && !result.Value) return;
            }

            var configurationOptionsWindow = new Wind0ws.ConfigurationOptionsWindow();

            configurationOptionsWindow.ShowDialog();
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

                AnimationMediaElement.Stop();
                AnimationMediaElement.Close();

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
