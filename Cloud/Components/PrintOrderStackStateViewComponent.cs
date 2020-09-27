using Microsoft.AspNetCore.Mvc;

namespace EM.Cloud.Components
{
    public class PrintOrderStackStateViewComponent : ViewComponent
    {
        private readonly Managers.PrintOrderStackStateManager printOrderStackStateManager;

        public PrintOrderStackStateViewComponent(Managers.PrintOrderStackStateManager printOrderStackStateManager)
        {
            this.printOrderStackStateManager = printOrderStackStateManager;
        }

        public IViewComponentResult Invoke()
        {
            return View(printOrderStackStateManager.Enabled);
        }
    }
}
