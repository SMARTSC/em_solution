using EM.Cloud.Extensions;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Authenticate([FromBody]Cloud.Api.Common.Models.AuthenticationIntentModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var userIdentity = identityProvider.ProvideIdentity(inputModel.User, inputModel.Password, Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);

                if (userIdentity != null)
                {
                    if (userIdentity.HasClaim(match => match.Type.Equals(SmartSolucionesCuba.SAPRESSC.Core.Web.Security.ClaimTypes.AccountDisabled) && match.Value.Equals(true.ToString())))
                    {
                        return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
                    }

                    return Json(userIdentity.ToAuthenticatedSessionModel(jwtOptions));
                }

                return Unauthorized(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
            }

            return BadRequest();
        }

        [HttpPost("[action]")]
        public ActionResult AuthenticateAsGuest([FromBody]Cloud.Api.Common.Models.GuestAuthenticationInputModel inputModel)
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
                    var claims = new System.Collections.Generic.List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultNameClaimType, terminal.Id),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType, Security.Authorization.Roles.ContentViewer),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType, Security.Authorization.Roles.AlbumViewer)
                    };

                    var guestIndentity = new System.Security.Claims.ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);

                    return Json(guestIndentity.ToAuthenticatedSessionModel(jwtOptions));
                }

                return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
            }

            return BadRequest();
        }
    }
}
