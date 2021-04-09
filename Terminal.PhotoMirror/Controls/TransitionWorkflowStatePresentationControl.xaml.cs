namespace EM.Terminal.PhotoMirror.Controls
{
    [System.ComponentModel.DesignTimeVisible(false)]
    [System.ComponentModel.ToolboxItem(false)]
    public partial class TransitionWorkflowStatePresentationControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        private System.Action transitionEndCallbackAction;

        public TransitionWorkflowStatePresentationControl(System.Uri transitionVideoSourceUri, System.Action transitionEndCallbackAction)
        {
            this.transitionEndCallbackAction = transitionEndCallbackAction;

            InitializeComponent();

            AnimationMediaElement.Source = transitionVideoSourceUri;
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

                transitionEndCallbackAction = null;

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

        private void AnimationMediaElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            transitionEndCallbackAction?.Invoke();
        }
    }
}
