namespace EM.Cloud.Api.Common.Models
{
    public class AuthenticatedSessionModel
    {
        public string Token { get; set; }

        public string Type { get; set; }

        public int ExpiresInSecond { get; set; }
    }
}
