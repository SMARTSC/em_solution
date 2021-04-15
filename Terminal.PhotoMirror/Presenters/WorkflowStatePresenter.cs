namespace EM.Terminal.PhotoMirror.Presenters
{
    internal sealed class WorkflowStatePresenter
    {
        private static WorkflowStatePresenter instance;

        internal static WorkflowStatePresenter Instance 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new WorkflowStatePresenter();
                }

                return instance;
            } 
        }

        private System.Windows.Controls.Panel wrapContainer;

        private System.Windows.Controls.UserControl presentedControl;

        private WorkflowStatePresenter() { }

        internal void BeginPresentation(System.Windows.Controls.Panel wrapContainer)
        {
            this.wrapContainer = wrapContainer;

            PresentSetUpWorkflowState();
        }

        internal void PresentUnspectedExceptionFoundWorkflowState(System.Exception exception)
        {
            PresentHelloWorkflowState();

            using (var eventLog = new System.Diagnostics.EventLog
            {
                Source = "Espejo Molón PhotoMirror"
            })
            {
                eventLog.WriteEntry(string.Format("Excepción no esperada: {0}", exception?.StackTrace ?? "Tiempo de espera en tarea asincrónica superado."), System.Diagnostics.EventLogEntryType.Error);
            }

            System.Windows.MessageBox.Show(string.Format("Se ha producido un error no esperado.{0}{0}Por favor notifique al personal responsable de esta situación.{0}{0}Disculpe las molestas causadas.", System.Environment.NewLine), "Aviso", System.Windows.MessageBoxButton.OK);
        }

        internal void PresentSetUpWorkflowState()
        {
            CleanWrapContainer();

            presentedControl = new Controls.SetupWorkflowStatePresentationControl();

            wrapContainer.Children.Add(presentedControl);
        }

        internal void PresentHelloWorkflowState()
        {
            CleanWrapContainer();

            presentedControl = new Controls.HelloWorkflowStatePresentationControl();

            wrapContainer.Children.Add(presentedControl);
        }

        internal void PresentContentCreationModePickWorkflowState()
        {
            CleanWrapContainer();

            presentedControl = new Controls.ContentCreationModePickStatePresentationControl();

            wrapContainer.Children.Add(presentedControl);
        }

        internal void PresentTransitionWorkflowState(System.Uri transitionVideoSourceUri, System.Action transitionEndCallbackAction)
        {
            CleanWrapContainer();

            presentedControl = new Controls.TransitionWorkflowStatePresentationControl(transitionVideoSourceUri, transitionEndCallbackAction);

            wrapContainer.Children.Add(presentedControl);
        }

        internal void PresentCameraOuturatorSimulationWorkflowState(Managers.Capture.Facade.IContentCaptureManagerFacade contentCaptureManager)
        {
            //CleanWrapContainer();

            //presentedControl = new Controls.CameraOuturatorSimulationStatePresentationControl(contentCaptureManager);

            //wrapContainer.Children.Add(presentedControl);

            PresentTransitionWorkflowState(new System.Uri(string.Format("Resources/Video/CameraCaptureComplete-{0}.mp4", new System.Random().Next(1, 3)), System.UriKind.Relative), contentCaptureManager.PerformNextWorkflowStep);
        }

        internal void PresentPictureContentReviewWorkflowState(System.Guid captureImageId)
        {
            CleanWrapContainer();

            presentedControl = new Controls.PictureContentReviewControlStatePresentationControl(captureImageId);

            wrapContainer.Children.Add(presentedControl);
        }

        internal void PresentPhotoSessionContentReviewWorkflowState(System.Guid[] captureImageIds)
        {
            CleanWrapContainer();

            presentedControl = new Controls.PhotoSessionContentReviewStatePresentationControl(captureImageIds);

            wrapContainer.Children.Add(presentedControl);
        }

        private void CleanWrapContainer()
        {
            if (presentedControl == null) return;

            wrapContainer.Children.Clear();

            if (presentedControl is System.IDisposable)
            {
                (presentedControl as System.IDisposable).Dispose();
            }

            presentedControl.Content = null;

            presentedControl = null;

            System.GC.Collect();
        }
    }
}
