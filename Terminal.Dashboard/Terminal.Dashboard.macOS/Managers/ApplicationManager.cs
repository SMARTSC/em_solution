[assembly: Xamarin.Forms.Dependency(typeof(EM.Terminal.Dashboard.macOS.Managers.ApplicationManager))]

namespace EM.Terminal.Dashboard.macOS.Managers
{
    public class ApplicationManager : Dashboard.Managers.Facade.IApplicationManagerFacade
    {
        public void Quit()
        {
            AppKit.NSApplication.SharedApplication.Terminate(null);
        }
    }
}
