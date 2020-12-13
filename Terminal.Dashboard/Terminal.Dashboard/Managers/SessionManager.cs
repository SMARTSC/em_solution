namespace EM.Terminal.Dashboard.Managers
{
    public sealed class SessionManager : SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade
    {
        private const string LAST_CLOUD_SERVER_KEY = "last-cloud-server";

        private static SessionManager instance;

        public static SessionManager Instance
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

        public string Server { get; private set; }
        public string AuthenticationType { get; private set; }
        public string AuthorizationToken { get; private set; }
        public string UserLogin { get; private set; }

        public bool HasAuthenticatedIdentity => UserLogin != null;

        private System.Collections.Generic.KeyValuePair<string, string> AuthorizeHeader
        {
            get
            {
                if (!HasAuthenticatedIdentity)
                {
                    return new System.Collections.Generic.KeyValuePair<string, string>();
                }
                else
                {
                    var type = AuthenticationType;
                    var token = AuthorizationToken;

                    return new System.Collections.Generic.KeyValuePair<string, string>(type, token);
                }
            }
        }

        public async System.Threading.Tasks.Task SignInAsync(string server, string login, string password)
        {
            Server = server;

            var securityApiClient = new Cloud.Api.Connect.Clients.SecurityApiClient(HttpClientHelper);

            var result = await securityApiClient.AuthenticateAsync(new Cloud.Api.Common.Models.AuthenticationIntentModel
            {
                User = login,
                Password = password
            });

            Xamarin.Forms.Application.Current.Properties[LAST_CLOUD_SERVER_KEY] = server;

            await Xamarin.Forms.Application.Current.SavePropertiesAsync();

            AuthenticationType = result.Type;
            AuthorizationToken = result.Token;
            UserLogin = login;
        }

        public void SignOut()
        {
            AuthenticationType = null;
            AuthorizationToken = null;
            UserLogin = null;
        }

        #region SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade implementation
        public SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper HttpClientHelper => new SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper(string.Format("http://{0}/api/", Server), () => AuthorizeHeader, null);
        #endregion
    }
}
