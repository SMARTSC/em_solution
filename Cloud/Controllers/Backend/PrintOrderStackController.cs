using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;

namespace EM.Cloud.Controllers.Backend
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequiresRoleBasePermission(SmartSolucionesCuba.SAPRESSC.Core.Web.Security.SecurityRoleCodes.ShellManager)]
    [Route("management/[area]/[controller]")]
    [Area("business")]
    public class PrintOrderStackController : SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly Managers.PrintOrderStackStateManager printOrderStackStateManager;

        public PrintOrderStackController(Managers.PrintOrderStackStateManager printOrderStackStateManager, Microsoft.Extensions.Localization.IStringLocalizer<PrintOrderStackController> localizer, ILogger<PrintOrderStackController> logger) : base(localizer, logger)
        {
            this.printOrderStackStateManager = printOrderStackStateManager;
        }


        [RequiresRoleBasePermission(Roles.PrintOrderStackManager)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TootleStatus()
        {
            printOrderStackStateManager.Enabled = !printOrderStackStateManager.Enabled;

            var notification = new SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.NotificationViewModel
            {
                Type = SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.NotificationType.Success,
                Message = printOrderStackStateManager.Enabled ? "Cola de Impresión habilitada con éxito." : "Cola de Impresión deshabilitada con éxito."
            };

            TempData[SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.NotificationViewModel.DEFAULT_NOTIFICATION_MAP_KEY] = Newtonsoft.Json.JsonConvert.SerializeObject(notification);

            return Redirect(Request.GetTypedHeaders().Referer.AbsoluteUri ?? "");
        }
    }
}
