using EM.Cloud.Backend.Api.Common.Models;
using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;

namespace EM.Cloud.Controllers.Backend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/backend/[controller]")]
    public class PrintOrdersController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<PrintOrder, long, PrintOrderInputModel, PrintOrderDisplayModel, Cloud.Backend.Api.Common.Models.Filter.PrintOrderFilterModel>
    {
        private readonly Managers.PrintOrderStackStateManager printOrderStackStateManager;

        public PrintOrdersController(Managers.PrintOrderStackStateManager printOrderStackStateManager, IEntityRepository<PrintOrder, long> repository, Microsoft.Extensions.Localization.IStringLocalizer<PrintOrdersController> localizer, ILogger<PrintOrdersController> logger) : base(repository, localizer, logger)
        {
            this.printOrderStackStateManager = printOrderStackStateManager;
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        [HttpGet]
        [Route("[action]")]
        public IActionResult StackEnabled()
        {
            return Ok(printOrderStackStateManager.Enabled);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult Get()
        {
            return base.Get();
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult Get(long key)
        {
            return base.Get(key);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult GetFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.PrintOrderFilterModel filterModel)
        {
            return base.GetFiltered(filterModel);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return base.GetPaginate(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult GetPaginateFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.PrintOrderFilterModel filterModel, int sheet = 1, int limit = 25)
        {
            return base.GetPaginateFiltered(filterModel, sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderCreator)]
        public override IActionResult Create([FromBody] PrintOrderInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderEditor)]
        public override IActionResult Edit([FromBody] PrintOrderInputModel inputModel)
        {
            return base.Edit(inputModel);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderRemover)]
        public override IActionResult Delete(long key)
        {
            return base.Delete(key);
        }
    }
}
