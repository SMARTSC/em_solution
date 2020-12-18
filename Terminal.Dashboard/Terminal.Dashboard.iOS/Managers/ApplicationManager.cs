[assembly: Xamarin.Forms.Dependency(typeof(EM.Terminal.Dashboard.iOS.Managers.ApplicationManager))]

namespace EM.Terminal.Dashboard.iOS.Managers
{
    public class ApplicationManager : Dashboard.Managers.Facade.IApplicationManagerFacade
    {
        public void Quit()
        {
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}
