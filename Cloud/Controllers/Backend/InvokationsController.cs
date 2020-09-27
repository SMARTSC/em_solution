using EM.Cloud.Models.View;
using EM.Cloud.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;

namespace EM.Cloud.Controllers.Backend
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequiresRoleBasePermission(SmartSolucionesCuba.SAPRESSC.Core.Web.Security.SecurityRoleCodes.ShellManager)]
    [Area("business")]
    public class InvokationsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Controllers.AbstractEntityManagementController<Invokation, string, InvokationInputViewModel, InvokationDisplayViewModel>
    {
        public InvokationsController(IEntityRepository<Invokation, string> repository, Microsoft.Extensions.Localization.IStringLocalizer<InvokationsController> localizer, ILogger<InvokationsController> logger) : base(repository, localizer, logger)
        {
        }

        [RequiresRoleBasePermission(Security.Authorization.Roles.InvocationViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        public override IActionResult Create()
        {
            return NotFound();
        }

        public override IActionResult Create(InvokationInputViewModel modelInput)
        {
            return NotFound();
        }

        public override IActionResult Edit(string key)
        {
            return NotFound();
        }

        public override IActionResult Edit(InvokationInputViewModel modelInput)
        {
            return NotFound();
        }

        public override IActionResult Delete(string key)
        {
            return NotFound();
        }

        public override IActionResult Delete(InvokationDisplayViewModel displayModel)
        {
            return NotFound();
        }
    }
}
