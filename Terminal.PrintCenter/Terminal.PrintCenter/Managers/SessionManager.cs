namespace EM.Terminal.PrintCenter.Managers
{
    public sealed class SessionManager
    {
        public SessionManager()
        {
            TouchInteractionMode = Enumerations.TouchInteractionMode.Resistive;
        }

        public string AuthenticationType { get; private set; }
        public string AuthorizationToken { get; private set; }
        public string UserLogin { get; private set; }

        public Cloud.Backend.Api.Common.Models.EventDisplayModel TargetEvent { get; set; }
        public Cloud.Backend.Api.Common.Models.AlbumDisplayModel TargetAlbum { get; set; }

        public Enumerations.TouchInteractionMode TouchInteractionMode { get; set; }

        public bool HasAuthenticatedSession => UserLogin != null;

        public bool HasTargetAlbum => TargetAlbum != null;

        public async System.Threading.Tasks.Task SignInAsync(string login, string password)
        {
            var securityApiConnectClient = new Cloud.Api.Connect.Clients.SecurityApiClient(Xamarin.Forms.DependencyService.Get<SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade>().HttpClientHelper);

            var result = await securityApiConnectClient.AuthenticateAsync(new Cloud.Api.Common.Models.AuthenticationIntentModel
            {
                User = login,
                Password = password
            });

            AuthenticationType = result.Type;
            AuthorizationToken = result.Token;
            UserLogin = login;
        }

        public void SignOut()
        {
            AuthenticationType = null;
            AuthorizationToken = null;
            UserLogin = null;
            TargetEvent = null;
            TargetAlbum = null;
        }
    }
}
