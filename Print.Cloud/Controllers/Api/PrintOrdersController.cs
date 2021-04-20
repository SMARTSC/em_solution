using EM.Print.Api.Common.Models;
using EM.Print.Api.Common.Models.Filter;
using EM.Print.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;

namespace EM.Print.Cloud.Controllers.Api.Business
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.ApiKeyAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[area]/[controller]")]
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

        public override IActionResult Get()
        {
            return base.Get();
        }

        public override IActionResult Get(long key)
        {
            return base.Get(key);
        }

        public override IActionResult GetFiltered([FromBody] PrintOrderFilterModel filterModel)
        {
            return base.GetFiltered(filterModel);
        }

        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return base.GetPaginate(sheet, limit);
        }

        public override IActionResult GetPaginateFiltered([FromBody] PrintOrderFilterModel filterModel, int sheet = 1, int limit = 25)
        {
            return base.GetPaginateFiltered(filterModel, sheet, limit);
        }

        public override IActionResult Create([FromBody] PrintOrderInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        public override IActionResult Edit([FromBody] PrintOrderInputModel inputModel)
        {
            return base.Edit(inputModel);
        }

        public override IActionResult Delete(long key)
        {
            return base.Delete(key);
        }
    }
}
