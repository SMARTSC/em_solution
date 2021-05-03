using EM.Print.Api.Common.Models;
using EM.Print.Api.Common.Models.Filter;
using EM.Print.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;

namespace EM.Print.Cloud.Controllers.Api.Business
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.ApiKeyAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PrintOrdersController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<PrintOrder, long, PrintOrderInputModel, PrintOrderDisplayModel, PrintOrderFilterModel>
    {
        private readonly Managers.PrintOrderStackStateManager printOrderStackStateManager;

        public PrintOrdersController(Managers.PrintOrderStackStateManager printOrderStackStateManager, IEntityRepository<PrintOrder, long> repository, Microsoft.Extensions.Localization.IStringLocalizer<PrintOrdersController> localizer, ILogger<PrintOrdersController> logger) : base(repository, localizer, logger)
        {
            this.printOrderStackStateManager = printOrderStackStateManager;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult StackEnabled()
        {
            return Ok(printOrderStackStateManager.Enabled);
        }
    }
}
