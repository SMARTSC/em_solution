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
    public class OrganizationClientsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Controllers.AbstractEntityManagementFilteredController<OrganizationClient, System.Guid, Models.View.OrganizationClientInputViewModel, Models.View.OrganizationClientDisplayViewModel, Models.Filter.OrganizationClientFilterModel>
    {
        private readonly SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager;
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.OrganizationIdentity, System.Guid> identityRepository;
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public OrganizationClientsController(Microsoft.Extensions.Configuration.IConfiguration configuration, SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager, IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.OrganizationIdentity, System.Guid> identityRepository, IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository, IEntityRepository<OrganizationClient, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<OrganizationClientsController> localizer, ILogger<OrganizationClientsController> logger) : base(repository, localizer, logger)
        {
            this.imageFileManager = imageFileManager;
            this.identityRepository = identityRepository;
            this.imageRepository = imageRepository;
            this.configuration = configuration;
        }

        protected override string GetFilterSessionID()
        {
            return "pbsp-business-organization-client-filterid";
        }

        [RequiresRoleBasePermission(Roles.ClientViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.ClientViewer)]
        public override IActionResult Filter(Models.Filter.OrganizationClientFilterModel filterModel)
        {
            return base.Filter(filterModel);
        }

        [RequiresRoleBasePermission(Roles.ClientCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequiresRoleBasePermission(Roles.ClientCreator)]
        public override IActionResult Create(Models.View.OrganizationClientInputViewModel modelInput)
        {
            if (Request.Form.Files.Count > 0)
            {
                var formFile = Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    ModelState.ClearValidationState("ImageFile");

                    modelInput.ImageFile = formFile;

                    var updateModelTask = TryUpdateModelAsync(modelInput);

                    updateModelTask.Wait();
                }
            }

            return base.Create(modelInput);
        }

        [RequiresRoleBasePermission(Roles.ClientEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequiresRoleBasePermission(Roles.ClientEditor)]
        public override IActionResult Edit(Models.View.OrganizationClientInputViewModel modelInput)
        {
            ModelState.ClearValidationState("ImageFile");
            ModelState.MarkFieldValid("ImageFile");

            if (Request.Form.Files.Count > 0)
            {
                var formFile = Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    modelInput.ImageFile = formFile;

                    var updateModelTask = TryUpdateModelAsync(modelInput);

                    updateModelTask.Wait();
                }
            }

            return base.Edit(modelInput);
        }

        [RequiresRoleBasePermission(Roles.ClientRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequiresRoleBasePermission(Roles.ClientRemover)]
        public override IActionResult Delete(Models.View.OrganizationClientDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        protected override void PreCreate(OrganizationClient entity, Models.View.OrganizationClientInputViewModel modelInput)
        {
            base.PreCreate(entity, modelInput);

            if (modelInput.ImageFile == null) return;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                modelInput.ImageFile.CopyTo(memoryStream);

                var image = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image { ContentType = modelInput.ImageFile.ContentType, Length = modelInput.ImageFile.Length, About = "Organization Client " + entity.Id };

                Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
                
                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), modelInput.ImageFile.ContentType);

                entity.Identity.Image = image;
            }
        }

        protected override void PreEdit(OrganizationClient entity, Models.View.OrganizationClientInputViewModel modelInput)
        {
            base.PreEdit(entity, modelInput);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            if (modelInput.ImageFile == null)
            {
                if (modelInput.ResetImage && entity.Identity.Image != null)
                {
                    imageFileManager.DeleteImageFile(entity.Identity.Image);

                    imageRepository.Delete(entity.Identity.Image, User.Identity.Name);
                }

                return;
            }

            if (entity.Identity.Image != null)
            {
                imageFileManager.DeleteImageFile(entity.Identity.Image);

                imageRepository.Delete(entity.Identity.Image, User.Identity.Name);
            }

            using (var memoryStream = new System.IO.MemoryStream())
            {
                modelInput.ImageFile.CopyTo(memoryStream);

                var image = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image { ContentType = modelInput.ImageFile.ContentType, Length = modelInput.ImageFile.Length, About = "Organization Client " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), modelInput.ImageFile.ContentType);

                entity.Identity.Image = image;
            }
        }

        protected override void PostDelete(OrganizationClient entity)
        {
            base.PostDelete(entity);

            identityRepository.Delete(entity.Identity, User.Identity.Name);

            if (entity.Identity.Image == null) return;

            imageRepository.Delete(entity.Identity.Image, User.Identity.Name);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            imageFileManager.DeleteImageFile(entity.Identity.Image);
        }

        protected override void ConfigurePostFailEditValidation(Models.View.OrganizationClientInputViewModel modelInput)
        {
            var entity = Repository.Get(modelInput.Id);

            if (entity.Identity.Image != null)
            {
                modelInput.ImageId = entity.Identity.Image.Id;
            }

            base.ConfigurePostFailEditValidation(modelInput);
        }

        protected override void ValidateCreate(Models.View.OrganizationClientInputViewModel modelInput)
        {
            base.ValidateCreate(modelInput);

            if (Repository.GetAll().Any(entity => entity.Identity.Name == modelInput.Name))
            {
                ModelState.AddModelError("Name", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Name"]]);
            }
        }

        protected override void ValidateEdit(Models.View.OrganizationClientInputViewModel modelInput)
        {
            base.ValidateEdit(modelInput);

            if (Repository.GetAll().Any(entity => entity.Id != modelInput.Id && entity.Identity.Name == modelInput.Name))
            {
                ModelState.AddModelError("Name", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Name"]]);
            }
        }
    }
}
