namespace EM.Terminal.PhotoMirror.Models.Window
{
    public class AuthenticationWindowBindableModel : BaseBindableModel
    {
        private bool isBusy;
        private string login;
        private string password;

        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        public string Login { get => login; set => SetProperty(ref login, value); }

        public string Password { get => password; set => SetProperty(ref password, value); }
    }
}
