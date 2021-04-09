namespace EM.Terminal.PhotoMirror
{
    public partial class App : System.Windows.Application
    {
        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            System.AppDomain.CurrentDomain.UnhandledException += (uunhandledExceptionSender, unhandledExceptionEventArgs) => OnGlobalUnhandleException((System.Exception)unhandledExceptionEventArgs.ExceptionObject);

            DispatcherUnhandledException += (unHandledExceptionSender, unHandledExceptionEventArgs) => OnGlobalUnhandleException(unHandledExceptionEventArgs.Exception);

            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (unHandledExceptionSender, unHandledExceptionEventArgs) => OnGlobalUnhandleException(unHandledExceptionEventArgs.Exception);
        }

        private void Application_Exit(object sender, System.Windows.ExitEventArgs eventArgs)
        {
            Managers.ContentUploadServiceManager.Instance.StopServiceIfRunning();
        }

        private void OnGlobalUnhandleException(System.Exception exception)
        {
            Presenters.WorkflowStatePresenter.Instance.PresentUnspectedExceptionFoundWorkflowState(exception);
        }
    }
}
