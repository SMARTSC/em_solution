namespace EM.Terminal.PhotoMirror.Managers.Capture
{
    public sealed class PictureContentCaptureManager : Facade.IContentCaptureManagerFacade
    {
        private System.Guid captureImageId;

        private void Success_Callback()
        {
            Presenters.WorkflowStatePresenter.Instance.PresentCameraOuturatorSimulationWorkflowState(this);
        }

        private void Fail_Callback(System.Exception exception)
        {
            Presenters.WorkflowStatePresenter.Instance.PresentUnspectedExceptionFoundWorkflowState(exception);
        }

        public void PerformNextWorkflowStep()
        {
            if (captureImageId == default)
            {
                captureImageId = System.Guid.NewGuid();

                Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri("Resources/Video/PictureContentPreCapture.mp4", System.UriKind.Relative), () => {
                    SessionManager.Instance.CaptureDevice.Capture(captureImageId, Success_Callback, Fail_Callback);
                });

                return;
            }

            Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri("Resources/Video/BeforeContentReview.mp4", System.UriKind.Relative), () =>
            {
                Presenters.WorkflowStatePresenter.Instance.PresentPictureContentReviewWorkflowState(captureImageId);
            });
        }
    }
}
