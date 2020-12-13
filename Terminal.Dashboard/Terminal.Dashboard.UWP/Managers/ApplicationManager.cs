[assembly: Xamarin.Forms.Dependency(typeof(EM.Terminal.Dashboard.UWP.Managers.ApplicationManager))]

namespace EM.Terminal.Dashboard.UWP.Managers
{
    public class ApplicationManager : Dashboard.Managers.Facade.IApplicationManagerFacade
    {
        public void Quit()
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }
    }
}
