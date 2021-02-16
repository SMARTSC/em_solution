using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EM.Cloud.Controllers
{
    [Area("security")]
    public class ErrorController : SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        public ErrorController(Microsoft.Extensions.Localization.IStringLocalizer<ErrorController> localizer, ILogger<ErrorController> logger) : base(localizer, logger)
        {
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View(new Models.View.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Forbidden()
        {
            SetDefaultNotification(SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View.NotificationType.Warning, "Acceso Prohibido", "No cuenta con los permisos suficientes para acceder al recurso solicitado.", null);

            return Redirect("/");
        }
    }
}
