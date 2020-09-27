using Microsoft.AspNetCore.Mvc;

namespace EM.Cloud.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("management");
        }
    }
}
