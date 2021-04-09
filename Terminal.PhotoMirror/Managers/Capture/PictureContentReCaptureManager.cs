namespace EM.Terminal.PhotoMirror.Managers.Capture
{
    public class PictureContentReCaptureManager : Facade.IContentCaptureManagerFacade
    {
        private bool isCaptureTakePlace;

        private System.Guid captureImageId;

        public PictureContentReCaptureManager(System.Guid captureImageId)
        {
            isCaptureTakePlace = false;

            this.captureImageId = captureImageId;
        }

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
            if (!isCaptureTakePlace)
            {
                Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri("Resources/Video/PictureContentPreReCapture.mp4", System.UriKind.Relative), () => {
                    isCaptureTakePlace = true;

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
