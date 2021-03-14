namespace EM.Terminal.Dashboard.Models.Page
{
    public class AlbumPasswordLockConfigurationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string password;

        public string Password { get => password; set => SetProperty(ref password, value); }
    }
}
