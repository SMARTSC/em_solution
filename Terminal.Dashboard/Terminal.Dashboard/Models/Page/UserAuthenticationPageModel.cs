namespace EM.Terminal.Dashboard.Models.Page
{
    public class UserAuthenticationPageModel : SmartSolucionesCuba.XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private string server;

        private string login;

        private string password;

        public string Server
        {
            get => server;
            set => SetProperty(ref server, value);
        }

        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
    }
}
