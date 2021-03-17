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
    public class TerminalsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Controllers.AbstractHabilitableEntityManagementController<Terminal, string, TerminalInputViewModel, TerminalDisplayViewModel>
    {
        private readonly IEntityRepository<Album, System.Guid> albumRepository;

        public TerminalsController(IEntityRepository<Album, System.Guid> albumRepository, IEntityRepository<Terminal, string> repository, Microsoft.Extensions.Localization.IStringLocalizer<TerminalsController> localizer, ILogger<TerminalsController> logger) : base(repository, localizer, logger)
        {
            this.albumRepository = albumRepository;
        }

        [RequiresRoleBasePermission(Security.Authorization.Roles.TerminalViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        public override IActionResult Create()
        {
            return NotFound();
        }

        public override IActionResult Create(TerminalInputViewModel modelInput)
        {
            return NotFound();
        }

        public override IActionResult Edit(string key)
        {
            return NotFound();
        }

        public override IActionResult Edit(TerminalInputViewModel modelInput)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Security.Authorization.Roles.TerminalEditor)]
        public override IActionResult Enable(string key)
        {
            return base.Enable(key);
        }

        [RequiresRoleBasePermission(Security.Authorization.Roles.TerminalEditor)]
        public override IActionResult Disable(string key)
        {
            return base.Disable(key);
        }

        public override IActionResult Delete(string key)
        {
            return NotFound();
        }

        public override IActionResult Delete(TerminalDisplayViewModel displayModel)
        {
            return NotFound();
        }

        protected override void PopulateViewModelDisplayExtras(TerminalDisplayViewModel modelDisplay, Terminal entity)
        {
            base.PopulateViewModelDisplayExtras(modelDisplay, entity);

            foreach (var bridge in entity.AlbumBridges)
            {
                var model = new AlbumDisplayViewModel();

                model.Import(albumRepository.Get(bridge.AlbumId));

                modelDisplay.Albums.Add(model);
            }
        }
    }
}
