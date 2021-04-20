namespace EM.Print.Cloud.Models
{
    public class ApiKeySessionModel : SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.Models.IApiKeySessionModel
    {
        public ApiKeySessionModel(string key, string ownerName)
        {
            Key = key;
            OwnerName = ownerName;
        }

        public string Key { get; private set; }

        public string OwnerName { get; private set; }

        public System.Collections.Generic.IReadOnlyCollection<System.Security.Claims.Claim> Claims { get; private set; }
    }
}
