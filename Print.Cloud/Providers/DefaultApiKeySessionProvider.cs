namespace EM.Print.Cloud.Providers
{
    public class DefaultApiKeySessionProvider : SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.Providers.Facade.IApiKeySessionProviderFacade
    {
        private readonly Microsoft.Extensions.Options.IOptions<Configuration.ApiKeyAuthenticationOptions> apiKeyAuthenticationOptions;

        public DefaultApiKeySessionProvider(Microsoft.Extensions.Options.IOptions<Configuration.ApiKeyAuthenticationOptions> apiKeyAuthenticationOptions)
        {
            this.apiKeyAuthenticationOptions = apiKeyAuthenticationOptions;
        }

        public async System.Threading.Tasks.Task<SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.Models.IApiKeySessionModel> ProvideAsync(string key)
        {
            if (apiKeyAuthenticationOptions.Value.Key != key) return null;

            return await System.Threading.Tasks.Task.FromResult(new Models.ApiKeySessionModel(key, System.Guid.NewGuid().ToString()));
        }
    }
}
