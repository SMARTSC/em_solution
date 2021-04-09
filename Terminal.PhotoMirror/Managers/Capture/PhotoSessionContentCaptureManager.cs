namespace EM.Terminal.PhotoMirror.Managers.Capture
{
    public class PhotoSessionContentCaptureManager : Facade.IContentCaptureManagerFacade
    {
        private System.Collections.Generic.List<System.Guid> captureImageIdList;

        public PhotoSessionContentCaptureManager()
        {
            captureImageIdList = new System.Collections.Generic.List<System.Guid>();
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
            var defaultPhotoSessionContentPrintLayout = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptorById(SessionManager.Instance.TargetAlbum.DefaultPhotoSessionContentCollageLayout.Id);

            if (captureImageIdList.Count < defaultPhotoSessionContentPrintLayout.PictureCount)
            {
                var captureImageId = System.Guid.NewGuid();

                captureImageIdList.Add(captureImageId);

                if (captureImageIdList.Count == 1)
                {
                    Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri(string.Format("Resources/Video/PhotoSessionContentPresentCapture-{0}.mp4", defaultPhotoSessionContentPrintLayout.PictureCount), System.UriKind.Relative), () =>
                    {
                        SessionManager.Instance.CaptureDevice.Capture(captureImageId, Success_Callback, Fail_Callback);
                    });
                }
                else
                {
                    Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri(string.Format("Resources/Video/PhotoSessionContentContinueCapture-{0}.mp4", new System.Random().Next(1, 4)), System.UriKind.Relative), () =>
                    {
                        SessionManager.Instance.CaptureDevice.Capture(captureImageId, Success_Callback, Fail_Callback);

                        return;
                    });
                }

                return;
            }

            Presenters.WorkflowStatePresenter.Instance.PresentTransitionWorkflowState(new System.Uri("Resources/Video/BeforeContentReview.mp4", System.UriKind.Relative), () =>
            {
                Presenters.WorkflowStatePresenter.Instance.PresentPhotoSessionContentReviewWorkflowState(captureImageIdList.ToArray());
            });
        }
    }
}
