using EM.Cloud.Api.Common.Models;
using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;

namespace EM.Cloud.Controllers.Api.Business
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[area]/[controller]")]
    [Area("business")]
    public class EventsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<Event, System.Guid, EventInputModel, EventDisplayModel, Models.Filter.EventFilterModel>
    {
        public EventsController(IEntityRepository<Event, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<EventsController> localizer, ILogger<EventsController> logger) : base(repository, localizer, logger)
        {
        }

        [RequiresRoleBasePermission(Roles.EventCreator)]
        public override IActionResult Create([FromBody] EventInputModel inputModel)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.EventRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.EventEditor)]
        public override IActionResult Edit([FromBody] EventInputModel inputModel)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult Get()
        {
            return base.Get();
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return base.GetPaginate(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult GetFiltered([FromBody] Models.Filter.EventFilterModel filterModel)
        {
            return base.GetFiltered(filterModel);
        }
    }
}
