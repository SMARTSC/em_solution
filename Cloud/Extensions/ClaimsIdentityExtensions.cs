using Microsoft.IdentityModel.Tokens;

namespace EM.Cloud.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static Api.Common.Models.AuthenticatedSessionModel ToAuthenticatedSessionModel(this System.Security.Claims.ClaimsIdentity userIdentity, Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions)
        {
            var token = GenerateJwtToken(userIdentity, jwtOptions);

            return new Api.Common.Models.AuthenticatedSessionModel
            {
                Token = token,
                Type = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                ExpiresInSecond = jwtOptions.Value.ExpiresInSecond
            };
        }

        private static string GenerateJwtToken(System.Security.Claims.ClaimsIdentity userIdentity, Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = System.DateTime.Now.AddSeconds(jwtOptions.Value.ExpiresInSecond);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                jwtOptions.Value.Issuer,
                jwtOptions.Value.Issuer,
                userIdentity.Claims,
                expires: expires,
                signingCredentials: creds
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
