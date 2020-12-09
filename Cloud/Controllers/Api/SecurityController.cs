using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Providers.Facade;

namespace EM.Cloud.Controllers.Api
{
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        private readonly IIdentityProviderFacade identityProvider;
        private readonly Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions;
        private readonly SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository;

        public SecurityController(SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository, IIdentityProviderFacade identityProvider, Microsoft.Extensions.Options.IOptions<Options.JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions;
            this.identityProvider = identityProvider;
            this.terminalRepository = terminalRepository;
        }

        [HttpPost("[action]")]
        public ActionResult Authenticate([FromBody]Cloud.Api.Common.Models.CloudAuthenticationInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = identityProvider.ProvideIdentity(inputModel.User, inputModel.Password, Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);

                if (userIdentity != null)
                {
                    if (userIdentity.HasClaim(match => match.Type.Equals(SmartSolucionesCuba.SAPRESSC.Core.Web.Security.ClaimTypes.AccountDisabled) && match.Value.Equals(true.ToString())))
                    {
                        return Forbid();
                    }

                    return GenerateToken(userIdentity);
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [HttpPost("[action]")]
        public ActionResult AuthenticateAsGuest([FromBody]Cloud.Api.Common.Models.CloudGuestAuthenticationInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var terminal = terminalRepository.FindOneBy(e => e.Id == inputModel.TerminalId);

                if (terminal == null)
                {
                    terminal = terminalRepository.Add(new Persistence.Entities.Terminal
                    {
                        Id = inputModel.TerminalId,
                        Enabled = true,
                        DeviceBrand = inputModel.DeviceBrand,
                        DeviceModel = inputModel.DeviceModel,
                        DeviceOS = inputModel.DeviceOS,
                        DeviceOSVersion = inputModel.DeviceOSVersion,
                        Version = inputModel.Version
                    }, "system");
                }

                if (terminal.Enabled)
                {
                    var claims = new System.Collections.Generic.List<System.Security.Claims.Claim>();

                    claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultNameClaimType, terminal.Id));

                    claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType, Security.Authorization.Roles.ContentViewer));
                    claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType, Security.Authorization.Roles.AlbumViewer));

                    var guestIndentity = new System.Security.Claims.ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);

                    return GenerateToken(guestIndentity);
                }

                return Forbid();
            }

            return BadRequest();
        }

        #region Token Utility Methods

        private ActionResult GenerateToken(System.Security.Claims.ClaimsIdentity userIdentity)
        {
            var token = GenerateJwtToken(userIdentity);

            return Json(new EM.Cloud.Api.Common.Models.CloudAuthenticationDisplayModel
            {
                Token = token,
                Type = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                ExpiresInSecond = jwtOptions.Value.ExpiresInSecond
            });
        }

        private string GenerateJwtToken(System.Security.Claims.ClaimsIdentity userIdentity)
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

        #endregion
    }
}
