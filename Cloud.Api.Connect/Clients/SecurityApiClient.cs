using EM.Cloud.Api.Common.Models;

namespace EM.Cloud.Api.Connect.Clients
{
    public class SecurityApiClient
    {
        protected readonly SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper;

        public SecurityApiClient(SmartSolucionesCuba.SAPRESSC.Core.Api.Connect.Helpers.HttpClientHelper httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }

        public async System.Threading.Tasks.Task<AuthenticatedSessionModel> AuthenticateAsync(AuthenticationIntentModel modelInput)
        {
            return await httpClientHelper.PostAsync<AuthenticationIntentModel, AuthenticatedSessionModel>("security/authenticate", modelInput);
        }
    }
}