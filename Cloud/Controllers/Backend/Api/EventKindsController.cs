using EM.Cloud.Backend.Api.Common.Models;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;

namespace EM.Cloud.Controllers.Backend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/backend/[controller]")]
    [ApiController]
    public class EventKindsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<Persistence.Entities.EventKind, System.Guid, EventKindInputModel, EventKindDisplayModel>
    {
        public EventKindsController(IEntityRepository<Persistence.Entities.EventKind, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<EventKindsController> localizer, ILogger<EventKindsController> logger) : base(repository, localizer, logger)
        {
        }

        [RequiresRoleBasePermission(Roles.EventKindViewer)]
        public override IActionResult Get()
        {
            return base.Get();
        }

        [RequiresRoleBasePermission(Roles.EventKindViewer)]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        [RequiresRoleBasePermission(Roles.EventKindViewer)]
        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return base.GetPaginate(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.EventKindCreator)]
        public override IActionResult Create([FromBody] EventKindInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        [RequiresRoleBasePermission(Roles.EventKindEditor)]
        public override IActionResult Edit([FromBody] EventKindInputModel inputModel)
        {
            return base.Edit(inputModel);
        }

        [RequiresRoleBasePermission(Roles.EventKindRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }
    }
}
