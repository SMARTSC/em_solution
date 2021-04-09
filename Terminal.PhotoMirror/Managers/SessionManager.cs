using EM.Cloud.Api.Common.Models;
using EM.Cloud.Backend.Api.Common.Models;

namespace EM.Terminal.PhotoMirror.Managers
{
    internal sealed class SessionManager
    {
        private static SessionManager instance;

        internal static SessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionManager();
                }

                return instance;
            }
        }

        private SessionManager() {}

        public string AuthenticationType { get; private set; }
        public string AuthorizationToken { get; private set; }
        public string UserLogin { get; private set; }

        public EventDisplayModel TargetEvent { get; set; }
        public AlbumDisplayModel TargetAlbum { get; set; }
        public Devices.Facade.ICaptureDeviceFacade CaptureDevice { get; set; }
        public char[] ConfigurationSecurityCode { get; set; }

        public bool HasAuthenticatedSession => UserLogin != null;

        public bool HasContentHandlingConfiguration => TargetAlbum != null && CaptureDevice != null;

        public async System.Threading.Tasks.Task SignInAsync(string login, string password)
        {
            var securityApiConnectClient = new Cloud.Api.Connect.Clients.SecurityApiClient(Providers.HttpClientHelperProvider.Instance.HttpClientHelper);

            var result = await securityApiConnectClient.AuthenticateAsync(new AuthenticationIntentModel
            {
                User = login,
                Password = password
            });

            AuthenticationType = result.Type;
            AuthorizationToken = result.Token;
            UserLogin = login;
        }
    }
}
