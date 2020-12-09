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
    public class PersonClientsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Controllers.AbstractEntityManagementFilteredController<PersonClient, System.Guid, Models.View.PersonClientInputViewModel, Models.View.PersonClientDisplayViewModel, Models.Filter.PersonClientFilterModel>
    {
        private readonly SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager;
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity, System.Guid> identityRepository;
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public PersonClientsController(Microsoft.Extensions.Configuration.IConfiguration configuration, SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManager, IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity, System.Guid> identityRepository, IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository, IEntityRepository<PersonClient, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<PersonClientsController> localizer, ILogger<PersonClientsController> logger) : base(repository, localizer, logger)
        {
            this.imageFileManager = imageFileManager;
            this.identityRepository = identityRepository;
            this.imageRepository = imageRepository;
            this.configuration = configuration;
        }

        protected override string GetFilterSessionID()
        {
            return "pbsp-business-person-client-filterid";
        }

        [RequiresRoleBasePermission(Roles.ClientViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.ClientViewer)]
        public override IActionResult Filter(Models.Filter.PersonClientFilterModel filterModel)
        {
            return base.Filter(filterModel);
        }

        [RequiresRoleBasePermission(Roles.ClientCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequiresRoleBasePermission(Roles.ClientCreator)]
        public override IActionResult Create(Models.View.PersonClientInputViewModel modelInput)
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
        public override IActionResult Edit(Models.View.PersonClientInputViewModel modelInput)
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
        public override IActionResult Delete(Models.View.PersonClientDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        protected override void PreCreate(PersonClient entity, Models.View.PersonClientInputViewModel modelInput)
        {
            base.PreCreate(entity, modelInput);

            if (modelInput.ImageFile == null) return;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                modelInput.ImageFile.CopyTo(memoryStream);

                var image = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image { ContentType = modelInput.ImageFile.ContentType, Length = modelInput.ImageFile.Length, About = "Person Client " + entity.Id };

                Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), modelInput.ImageFile.ContentType);

                entity.Identity.Image = image;
            }
        }

        protected override void PreEdit(PersonClient entity, Models.View.PersonClientInputViewModel modelInput)
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

                var image = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image { ContentType = modelInput.ImageFile.ContentType, Length = modelInput.ImageFile.Length, About = "Person Client " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), modelInput.ImageFile.ContentType);

                entity.Identity.Image = image;
            }
        }

        protected override void PostDelete(PersonClient entity)
        {
            base.PostDelete(entity);

            identityRepository.Delete(entity.Identity, User.Identity.Name);

            if (entity.Identity.Image == null) return;

            imageRepository.Delete(entity.Identity.Image, User.Identity.Name);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            imageFileManager.DeleteImageFile(entity.Identity.Image);
        }

        protected override void ConfigurePostFailEditValidation(Models.View.PersonClientInputViewModel modelInput)
        {
            var entity = Repository.Get(modelInput.Id);

            if (entity.Identity.Image != null)
            {
                modelInput.ImageId = entity.Identity.Image.Id;
            }

            base.ConfigurePostFailEditValidation(modelInput);
        }

        protected override void ValidateCreate(Models.View.PersonClientInputViewModel modelInput)
        {
            base.ValidateCreate(modelInput);

            if (Repository.GetAll().Any(entity => entity.Identity.Name == modelInput.Name && entity.Identity.LastName == modelInput.LastName))
            {
                ModelState.AddModelError("Name", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Name"], localizer["LastName"]]);
                ModelState.AddModelError("LastName", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Name"], localizer["LastName"]]);
            }
        }

        protected override void ValidateEdit(Models.View.PersonClientInputViewModel modelInput)
        {
            base.ValidateEdit(modelInput);

            if (Repository.GetAll().Any(entity => entity.Id != modelInput.Id && entity.Identity.Name == modelInput.Name && entity.Identity.LastName == modelInput.LastName))
            {
                ModelState.AddModelError("Name", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Name"], localizer["LastName"]]);
                ModelState.AddModelError("LastName", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Name"], localizer["LastName"]]);
            }
        }
    }
}
