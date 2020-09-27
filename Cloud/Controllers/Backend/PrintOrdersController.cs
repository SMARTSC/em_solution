using EM.Cloud.Models.View;
using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;
using System.Linq;

namespace EM.Cloud.Controllers.Backend
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequiresRoleBasePermission(SmartSolucionesCuba.SAPRESSC.Core.Web.Security.SecurityRoleCodes.ShellManager)]
    [Area("business")]
    public class PrintOrdersController : SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Controllers.AbstractHabilitableEntityManagementFilteredController<PrintOrder, long, PrintOrderInputViewModel, PrintOrderDisplayViewModel, Models.Filter.PrintOrderFilterModel>
    {
        public PrintOrdersController(IEntityRepository<PrintOrder, long> repository, Microsoft.Extensions.Localization.IStringLocalizer<PrintOrdersController> localizer, ILogger<PrintOrdersController> logger) : base(repository, localizer, logger)
        {
        }

        protected override string GetFilterSessionID()
        {
            return "pbsp-business-printorder-filterid";
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderViewer)]
        public override IActionResult Filter(Models.Filter.PrintOrderFilterModel filterModel)
        {
            return base.Filter(filterModel);
        }

        public override IActionResult Create()
        {
            return NotFound();
        }

        public override IActionResult Create(PrintOrderInputViewModel modelInput)
        {
            return NotFound();
        }

        public override IActionResult Edit(long key)
        {
            return NotFound();
        }

        public override IActionResult Edit(PrintOrderInputViewModel modelInput)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.PrintOrderEditor)]
        [HttpGet]
        [Route("[action]/{key}")]
        public IActionResult Overview(long key)
        {
            if (Request.Headers["X-Requested-With"] != "XMLHttpRequest") return NotFound();

            try
            {
                var entity = Repository.Get(key);

                var modelDisplay = new ExtendedPrintOrderDisplayViewModel();

                modelDisplay.Import(entity);

                return View(modelDisplay);
            }
            catch (System.Exception e)
            {
                logger.LogError(1, e.InnerException, e.Message, null);
            }

            SetDefaultNotification(SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [RequiresRoleBasePermission(Roles.PrintOrderEditor)]
        public override IActionResult Enable(long key)
        {
            return base.Enable(key);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderEditor)]
        public override IActionResult Disable(long key)
        {
            return base.Disable(key);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderRemover)]
        public override IActionResult Delete(long key)
        {
            return base.Delete(key);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderRemover)]
        public override IActionResult Delete(PrintOrderDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        [RequiresRoleBasePermission(Roles.PrintOrderRemover)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePendings()
        {
            var pendings = Repository.FindBy(entity => entity.Attended < entity.Created).ToArray();

            foreach (var pending in pendings)
            {
                Repository.Delete(pending, User.Identity.Name);
            }

            return RedirectToAction();
        }
    }
}
