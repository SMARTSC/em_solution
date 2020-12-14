namespace EM.Terminal.Dashboard.Helpers.Facade
{
    public interface IExceptionHandleHelperFacade
    {
        void HandleException(System.Exception exception, System.Action continuationCallback = null);
    }
}
