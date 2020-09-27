using EM.Cloud.Models.Filter;
using EM.Cloud.Models.View;
using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Common.Models.View;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;
using System.Collections.Generic;
using System.Linq;

namespace EM.Cloud.Controllers.Backend
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequiresRoleBasePermission(SmartSolucionesCuba.SAPRESSC.Core.Web.Security.SecurityRoleCodes.ShellManager)]
    [Area("business")]
    public class EventsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Backend.Controllers.AbstractEntityManagementFilteredController<Event, System.Guid, EventInputViewModel, EventDisplayViewModel, EventFilterModel>
    {
        private readonly SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManagerFacade;
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Album, System.Guid> albumRepository;
        private readonly IEntityRepository<CollageLayout, System.Guid> collageLayoutRepository;
        private readonly Microsoft.Extensions.Options.IOptions<Options.EventMediaDimensionContraintOptions> mediaDimensionContraintOptions;
        private readonly IEntityRepository<PictureContent, System.Guid> pictureContentRepository;

        public EventsController(IEntityRepository<PictureContent, System.Guid> pictureContentRepository, Microsoft.Extensions.Options.IOptions<Options.EventMediaDimensionContraintOptions> mediaDimensionContraintOptions, IEntityRepository<CollageLayout, System.Guid> collageLayoutRepository, SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade imageFileManagerFacade, IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository, IEntityRepository<Album, System.Guid> albumRepository, IEntityRepository<Event, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<EventsController> localizer, ILogger<EventsController> logger) : base(repository, localizer, logger)
        {
            this.imageFileManagerFacade = imageFileManagerFacade;
            this.imageRepository = imageRepository;
            this.albumRepository = albumRepository;
            this.collageLayoutRepository = collageLayoutRepository;
            this.mediaDimensionContraintOptions = mediaDimensionContraintOptions;
            this.pictureContentRepository = pictureContentRepository;
        }

        protected override string GetFilterSessionID()
        {
            return "pbsp-business-event-filterid";
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult Filter(EventFilterModel filterModel)
        {
            return base.Filter(filterModel);
        }

        [RequiresRoleBasePermission(Roles.EventCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequiresRoleBasePermission(Roles.EventCreator)]
        public override IActionResult Create(EventInputViewModel modelInput)
        {
            if (Request.Form.Files.Count > 0)
            {
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        ModelState.ClearValidationState(formFile.Name);

                        switch (formFile.Name)
                        {
                            case "LogoFile":
                                modelInput.LogoFile = formFile;

                                break;

                            //case "PrintLogoMiddleFile":
                            //    modelInput.PrintLogoMiddleFile = formFile;

                            //    break;

                            //case "PrintMiddleStrechLogoFile":
                            //    modelInput.PrintMiddleStrechLogoFile = formFile;

                            //    break;

                            //case "PrintLogoSmallFile":
                            //    modelInput.PrintLogoSmallFile = formFile;

                            //    break;

                            //case "PrintSmallStrechLogoFile":
                            //    modelInput.PrintSmallStrechLogoFile = formFile;

                            //    break;
                        }
                    }
                }

                TryUpdateModelAsync(modelInput).Wait();
            }

            return base.Create(modelInput);
        }

        [RequiresRoleBasePermission(Roles.EventEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequiresRoleBasePermission(Roles.EventEditor)]
        public override IActionResult Edit(EventInputViewModel modelInput)
        {   
            if (Request.Form.Files.Count > 0)
            {
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        ModelState.ClearValidationState(formFile.Name);

                        switch (formFile.Name)
                        {
                            case "LogoFile":
                                modelInput.LogoFile = formFile;

                                break;

                            //case "PrintLogoMiddleFile":
                            //    modelInput.PrintLogoMiddleFile = formFile;

                            //    break;

                            //case "PrintMiddleStrechLogoFile":
                            //    modelInput.PrintMiddleStrechLogoFile = formFile;

                            //    break;

                            //case "PrintLogoSmallFile":
                            //    modelInput.PrintLogoSmallFile = formFile;

                            //    break;

                            //case "PrintSmallStrechLogoFile":
                            //    modelInput.PrintSmallStrechLogoFile = formFile;

                            //    break;
                        }
                    }
                }

                TryUpdateModelAsync(modelInput).Wait();

                if (modelInput.LogoFile == null)
                {
                    ModelState.ClearValidationState("LogoFile");
                    ModelState.MarkFieldValid("LogoFile");
                }

                //if (modelInput.PrintLogoMiddleFile == null)
                //{
                //    ModelState.ClearValidationState("PrintLogoMiddleFile");

                //    ModelState.MarkFieldValid("PrintLogoMiddleFile");
                //}

                //if (modelInput.PrintMiddleStrechLogoFile == null)
                //{
                //    ModelState.ClearValidationState("PrintMiddleStrechLogoFile");

                //    ModelState.MarkFieldValid("PrintMiddleStrechLogoFile");
                //}

                //if (modelInput.PrintLogoSmallFile == null)
                //{
                //    ModelState.ClearValidationState("PrintLogoSmallFile");

                //    ModelState.MarkFieldValid("PrintLogoSmallFile");
                //}

                //if (modelInput.PrintSmallStrechLogoFile == null)
                //{
                //    ModelState.ClearValidationState("PrintSmallStrechLogoFile");

                //    ModelState.MarkFieldValid("PrintSmallStrechLogoFile");
                //}
            }

            return base.Edit(modelInput);
        }

        [RequiresRoleBasePermission(Roles.EventRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequiresRoleBasePermission(Roles.EventRemover)]
        public override IActionResult Delete(EventDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        protected override void PreCreate(Event entity, EventInputViewModel modelInput)
        {
            base.PreCreate(entity, modelInput);

            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintMiddleLogo = null;
            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintMiddleStrechLogo = null;
            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintSmallLogo = null;
            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintSmallStrechLogo = null;

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintLogoMiddleFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //    {
            //        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //        {
            //            if (imageSkBitmap.Width != imageSkBitmap.Height)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoMiddleFile", localizer["CuadrateImageRequiredMessage"]);
            //            }

            //            var max = mediaDimensionContraintOptions.Value.PrintMiddleLogo + mediaDimensionContraintOptions.Value.PrintMiddleLogoTolerance;
            //            var min = mediaDimensionContraintOptions.Value.PrintMiddleLogo;

            //            if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoMiddleFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //            }

            //            newPrintMiddleLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //            {
            //                ContentType = modelInput.PrintLogoMiddleFile.ContentType,
            //                Length = modelInput.PrintLogoMiddleFile.Length,
            //                Width = imageSkBitmap.Width,
            //                Height = imageSkBitmap.Height
            //            };
            //        }
            //    }
            //}

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintMiddleStrechLogoFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //    {
            //        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //        {
            //            if (imageSkBitmap.Width != imageSkBitmap.Height)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintMiddleStrechLogoFile", localizer["CuadrateImageRequiredMessage"]);
            //            }

            //            var max = mediaDimensionContraintOptions.Value.PrintMiddleLogoStrech + mediaDimensionContraintOptions.Value.PrintMiddleLogoStrechTolerance;
            //            var min = mediaDimensionContraintOptions.Value.PrintMiddleLogoStrech;

            //            if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintMiddleStrechLogoFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //            }

            //            newPrintMiddleStrechLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //            {
            //                ContentType = modelInput.PrintMiddleStrechLogoFile.ContentType,
            //                Length = modelInput.PrintMiddleStrechLogoFile.Length,
            //                Width = imageSkBitmap.Width,
            //                Height = imageSkBitmap.Height
            //            };
            //        }
            //    }
            //}

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintLogoSmallFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //    {
            //        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //        {
            //            if (imageSkBitmap.Width != imageSkBitmap.Height)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoSmallFile", localizer["CuadrateImageRequiredMessage"]);
            //            }

            //            var max = mediaDimensionContraintOptions.Value.PrintSmallLogo + mediaDimensionContraintOptions.Value.PrintSmallLogoTolerance;
            //            var min = mediaDimensionContraintOptions.Value.PrintSmallLogo;

            //            if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoSmallFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //            }

            //            newPrintSmallLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //            {
            //                ContentType = modelInput.PrintLogoSmallFile.ContentType,
            //                Length = modelInput.PrintLogoSmallFile.Length,
            //                Width = imageSkBitmap.Width,
            //                Height = imageSkBitmap.Height
            //            };
            //        }
            //    }
            //}

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintSmallStrechLogoFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //    {
            //        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //        {
            //            if (imageSkBitmap.Width != imageSkBitmap.Height)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintSmallStrechLogoFile", localizer["CuadrateImageRequiredMessage"]);
            //            }

            //            var max = mediaDimensionContraintOptions.Value.PrintSmallLogoStrech + mediaDimensionContraintOptions.Value.PrintSmallLogoStrechTolerance;
            //            var min = mediaDimensionContraintOptions.Value.PrintSmallLogoStrech;

            //            if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //            {
            //                throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintSmallStrechLogoFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //            }

            //            newPrintSmallStrechLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //            {
            //                ContentType = modelInput.PrintSmallStrechLogoFile.ContentType,
            //                Length = modelInput.PrintSmallStrechLogoFile.Length,
            //                Width = imageSkBitmap.Width,
            //                Height = imageSkBitmap.Height
            //            };
            //        }
            //    }
            //}

            using (var memoryStream = new System.IO.MemoryStream())
            {
                modelInput.LogoFile.CopyTo(memoryStream);

                entity.Logo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image { ContentType = modelInput.LogoFile.ContentType, Length = modelInput.LogoFile.Length, About = "Event Logo " + entity.Id };

                imageFileManagerFacade.SaveImageFileContent(entity.Logo.Id, memoryStream.ToArray(), modelInput.LogoFile.ContentType);
            }

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintLogoMiddleFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    entity.PrintMiddleLogo = newPrintMiddleLogo;

            //    entity.PrintMiddleLogo.About = string.Format("Event Print Middle Logo :{0}", entity.PrintMiddleLogo.Id);

            //    imageFileManagerFacade.SaveImageFileContent(entity.PrintMiddleLogo.Id, memoryStream.ToArray(), modelInput.PrintLogoMiddleFile.ContentType);
            //}

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintMiddleStrechLogoFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    entity.PrintMiddleStrechLogo = newPrintMiddleStrechLogo;

            //    entity.PrintMiddleStrechLogo.About = string.Format("Event Print Middle Strech Logo :{0}", entity.PrintMiddleStrechLogo.Id);

            //    imageFileManagerFacade.SaveImageFileContent(entity.PrintMiddleStrechLogo.Id, memoryStream.ToArray(), modelInput.PrintMiddleStrechLogoFile.ContentType);
            //}

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintLogoSmallFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    entity.PrintSmallLogo = newPrintSmallLogo;

            //    entity.PrintSmallLogo.About = string.Format("Event Print Small Logo :{0}", entity.PrintSmallLogo.Id);

            //    imageFileManagerFacade.SaveImageFileContent(entity.PrintSmallLogo.Id, memoryStream.ToArray(), modelInput.PrintLogoSmallFile.ContentType);
            //}

            //using (var memoryStream = new System.IO.MemoryStream())
            //{
            //    modelInput.PrintSmallStrechLogoFile.CopyTo(memoryStream);

            //    memoryStream.Position = 0;

            //    entity.PrintSmallStrechLogo = newPrintSmallStrechLogo;

            //    entity.PrintSmallStrechLogo.About = string.Format("Event Print Middle Strech Logo :{0}", entity.PrintSmallStrechLogo.Id);

            //    imageFileManagerFacade.SaveImageFileContent(entity.PrintSmallStrechLogo.Id, memoryStream.ToArray(), modelInput.PrintSmallStrechLogoFile.ContentType);
            //}
        }

        protected override void PreEdit(Event entity, EventInputViewModel modelInput)
        {
            base.PreEdit(entity, modelInput);

            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintMiddleLogo = null;
            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintMiddleStrechLogo = null;
            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintSmallLogo = null;
            //SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image newPrintSmallStrechLogo = null;

            //if (modelInput.PrintLogoMiddleFile != null)
            //{
            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintLogoMiddleFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //        {
            //            using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //            {
            //                if (imageSkBitmap.Width != imageSkBitmap.Height)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoMiddleFile", localizer["CuadrateImageRequiredMessage"]);
            //                }

            //                var max = mediaDimensionContraintOptions.Value.PrintMiddleLogo + mediaDimensionContraintOptions.Value.PrintMiddleLogoTolerance;
            //                var min = mediaDimensionContraintOptions.Value.PrintMiddleLogo;

            //                if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoMiddleFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //                }

            //                newPrintMiddleLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //                {
            //                    ContentType = modelInput.PrintLogoMiddleFile.ContentType,
            //                    Length = modelInput.PrintLogoMiddleFile.Length,
            //                    Width = imageSkBitmap.Width,
            //                    Height = imageSkBitmap.Height
            //                };
            //            }
            //        }
            //    }
            //}

            //if (modelInput.PrintMiddleStrechLogoFile != null)
            //{
            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintMiddleStrechLogoFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //        {
            //            using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //            {
            //                if (imageSkBitmap.Width != imageSkBitmap.Height)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintMiddleStrechLogoFile", localizer["CuadrateImageRequiredMessage"]);
            //                }

            //                var max = mediaDimensionContraintOptions.Value.PrintMiddleLogoStrech + mediaDimensionContraintOptions.Value.PrintMiddleLogoStrechTolerance;
            //                var min = mediaDimensionContraintOptions.Value.PrintMiddleLogoStrech;

            //                if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintMiddleStrechLogoFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //                }

            //                newPrintMiddleStrechLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //                {
            //                    ContentType = modelInput.PrintMiddleStrechLogoFile.ContentType,
            //                    Length = modelInput.PrintMiddleStrechLogoFile.Length,
            //                    Width = imageSkBitmap.Width,
            //                    Height = imageSkBitmap.Height
            //                };
            //            }
            //        }
            //    }
            //}

            //if (modelInput.PrintLogoSmallFile != null)
            //{
            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintLogoSmallFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //        {
            //            using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //            {
            //                if (imageSkBitmap.Width != imageSkBitmap.Height)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoSmallFile", localizer["CuadrateImageRequiredMessage"]);
            //                }

            //                var max = mediaDimensionContraintOptions.Value.PrintSmallLogo + mediaDimensionContraintOptions.Value.PrintSmallLogoTolerance;
            //                var min = mediaDimensionContraintOptions.Value.PrintSmallLogo;

            //                if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintLogoSmallFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //                }

            //                newPrintSmallLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //                {
            //                    ContentType = modelInput.PrintLogoSmallFile.ContentType,
            //                    Length = modelInput.PrintLogoSmallFile.Length,
            //                    Width = imageSkBitmap.Width,
            //                    Height = imageSkBitmap.Height
            //                };
            //            }
            //        }
            //    }
            //}

            //if (modelInput.PrintSmallStrechLogoFile != null)
            //{
            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintSmallStrechLogoFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        using (var imageSkData = SkiaSharp.SKData.Create(memoryStream))
            //        {
            //            using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
            //            {
            //                if (imageSkBitmap.Width != imageSkBitmap.Height)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintSmallStrechLogoFile", localizer["CuadrateImageRequiredMessage"]);
            //                }

            //                var max = mediaDimensionContraintOptions.Value.PrintSmallLogoStrech + mediaDimensionContraintOptions.Value.PrintSmallLogoStrechTolerance;
            //                var min = mediaDimensionContraintOptions.Value.PrintSmallLogoStrech;

            //                if (max < imageSkBitmap.Width || min > imageSkBitmap.Width)
            //                {
            //                    throw new SmartSolucionesCuba.SAPRESSC.Core.Web.Frontend.InvalidModelException("PrintSmallStrechLogoFile", string.Format(localizer["MediaDimensionContraintInvalidMessage"], min, max));
            //                }

            //                newPrintSmallStrechLogo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image
            //                {
            //                    ContentType = modelInput.PrintSmallStrechLogoFile.ContentType,
            //                    Length = modelInput.PrintSmallStrechLogoFile.Length,
            //                    Width = imageSkBitmap.Width,
            //                    Height = imageSkBitmap.Height
            //                };
            //            }
            //        }
            //    }
            //}

            if (modelInput.LogoFile != null)
            {
                var oldLogo = entity.Logo;

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    modelInput.LogoFile.CopyTo(memoryStream);

                    entity.Logo = new SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image { ContentType = modelInput.LogoFile.ContentType, Length = modelInput.LogoFile.Length, About = "Event " + entity.Id };

                    imageFileManagerFacade.SaveImageFileContent(entity.Logo.Id, memoryStream.ToArray(), modelInput.LogoFile.ContentType);
                }

                imageFileManagerFacade.DeleteImageFile(oldLogo);

                imageRepository.Delete(oldLogo, User.Identity.Name);
            }

            //if (newPrintMiddleLogo != null)
            //{
            //    var oldPrintMiddleLogo = entity.PrintMiddleLogo;

            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintLogoMiddleFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        entity.PrintMiddleLogo = newPrintMiddleLogo;

            //        entity.PrintMiddleLogo.About = string.Format("Event Print Middle Logo :{0}", entity.PrintMiddleLogo.Id);

            //        imageFileManagerFacade.SaveImageFileContent(entity.PrintMiddleLogo.Id, memoryStream.ToArray(), modelInput.PrintLogoMiddleFile.ContentType);
            //    }

            //    imageFileManagerFacade.DeleteImageFile(oldPrintMiddleLogo);

            //    imageRepository.Delete(oldPrintMiddleLogo, User.Identity.Name);
            //}

            //if (newPrintMiddleStrechLogo != null)
            //{
            //    var oldPrintMiddleStrechLogo = entity.PrintMiddleStrechLogo; 

            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintMiddleStrechLogoFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        entity.PrintMiddleStrechLogo = newPrintMiddleStrechLogo;

            //        entity.PrintMiddleStrechLogo.About = string.Format("Event Print Middle Strech Logo :{0}", entity.PrintMiddleStrechLogo.Id);

            //        imageFileManagerFacade.SaveImageFileContent(entity.PrintMiddleStrechLogo.Id, memoryStream.ToArray(), modelInput.PrintMiddleStrechLogoFile.ContentType);
            //    }

            //    imageFileManagerFacade.DeleteImageFile(oldPrintMiddleStrechLogo);

            //    imageRepository.Delete(oldPrintMiddleStrechLogo, User.Identity.Name);
            //}

            //if (newPrintSmallLogo != null)
            //{
            //    var oldPrintSmallLogo = entity.PrintSmallLogo;

            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintLogoSmallFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        entity.PrintSmallLogo = newPrintSmallLogo;

            //        entity.PrintSmallLogo.About = string.Format("Event Print Small Logo :{0}", entity.PrintSmallLogo.Id);

            //        imageFileManagerFacade.SaveImageFileContent(entity.PrintSmallLogo.Id, memoryStream.ToArray(), modelInput.PrintLogoSmallFile.ContentType);
            //    }

            //    imageFileManagerFacade.DeleteImageFile(oldPrintSmallLogo);

            //    imageRepository.Delete(oldPrintSmallLogo, User.Identity.Name);
            //}

            //if (newPrintSmallStrechLogo != null)
            //{
            //    var oldPrintSmallStrechLogo = entity.PrintSmallStrechLogo;

            //    using (var memoryStream = new System.IO.MemoryStream())
            //    {
            //        modelInput.PrintSmallStrechLogoFile.CopyTo(memoryStream);

            //        memoryStream.Position = 0;

            //        entity.PrintSmallStrechLogo = newPrintSmallStrechLogo;

            //        entity.PrintSmallStrechLogo.About = string.Format("Event Print Middle Strech Logo :{0}", entity.PrintSmallStrechLogo.Id);

            //        imageFileManagerFacade.SaveImageFileContent(entity.PrintSmallStrechLogo.Id, memoryStream.ToArray(), modelInput.PrintSmallStrechLogoFile.ContentType);
            //    }

            //    imageFileManagerFacade.DeleteImageFile(oldPrintSmallStrechLogo);

            //    imageRepository.Delete(oldPrintSmallStrechLogo, User.Identity.Name);
            //}
        }

        protected override void PostDelete(Event entity)
        {
            base.PostDelete(entity);

            if (entity.Logo == null) return;

            imageRepository.Delete(entity.Logo, User.Identity.Name);
            //imageRepository.Delete(entity.PrintMiddleLogo, User.Identity.Name);
            //imageRepository.Delete(entity.PrintMiddleStrechLogo, User.Identity.Name);
            //imageRepository.Delete(entity.PrintSmallLogo, User.Identity.Name);
            //imageRepository.Delete(entity.PrintSmallStrechLogo, User.Identity.Name);

            imageFileManagerFacade.DeleteImageFile(entity.Logo);
            //imageFileManagerFacade.DeleteImageFile(entity.PrintMiddleLogo);
            //imageFileManagerFacade.DeleteImageFile(entity.PrintMiddleStrechLogo);
            //imageFileManagerFacade.DeleteImageFile(entity.PrintSmallLogo);
            //imageFileManagerFacade.DeleteImageFile(entity.PrintSmallStrechLogo);
        }

        protected override void ConfigureCreate(EventInputViewModel modelInput)
        {
            base.ConfigureCreate(modelInput);

            modelInput.MediaDimensionContraintOptions = mediaDimensionContraintOptions.Value;
        }

        protected override void ConfigureEdit(Event entity, EventInputViewModel modelInput)
        {
            base.ConfigureEdit(entity, modelInput);

            modelInput.MediaDimensionContraintOptions = mediaDimensionContraintOptions.Value;
        }

        protected override void ConfigurePostFailCreateValidation(EventInputViewModel modelInput)
        {
            base.ConfigurePostFailCreateValidation(modelInput);

            modelInput.MediaDimensionContraintOptions = mediaDimensionContraintOptions.Value;
        }

        protected override void ConfigurePostFailEditValidation(EventInputViewModel modelInput)
        {
            modelInput.MediaDimensionContraintOptions = mediaDimensionContraintOptions.Value;

            var entity = Repository.Get(modelInput.Id);

            if (entity.Logo != null)
            {
                modelInput.LogoId = entity.Logo.Id;
            }

            //if (entity.PrintMiddleLogo != null)
            //{
            //    modelInput.PrintLogoMiddleId = entity.PrintMiddleLogo.Id;
            //}

            //if (entity.PrintMiddleStrechLogo != null)
            //{
            //    modelInput.PrintMiddleStrechLogoId = entity.PrintMiddleStrechLogo.Id;
            //}

            //if (entity.PrintSmallLogo != null)
            //{
            //    modelInput.PrintLogoSmallId = entity.PrintSmallLogo.Id;
            //}

            //if (entity.PrintSmallStrechLogo != null)
            //{
            //    modelInput.PrintSmallStrechLogoId = entity.PrintSmallStrechLogo.Id;
            //}
        }

        #region Event Albumns Management

        [Route("edit/{key}/albums")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        public IActionResult Albums(System.Guid key)
        {
            try
            {
                var entity = Repository.Get(key);

                var enabledCollageLayoutViewModels = collageLayoutRepository.FindBy(collageLayout => collageLayout.Enabled).ToList().Select((CollageLayout collageLayout) =>
                {
                    var descriptor = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptorById(collageLayout.Id);

                    var collageLayoutModel = new CollageLayoutDisplayViewModel();

                    collageLayoutModel.Import(collageLayout);

                    return collageLayoutModel;
                });

                var resultModel = new EventAlbumsViewModel
                {
                    Event = new EventDisplayViewModel(),
                    DefaultPictureContentCollageLayoutSelectListItems = enabledCollageLayoutViewModels.Where(collageLayoutDisplayViewModel => collageLayoutDisplayViewModel.PictureCount == 1).Select(collageLayoutDisplayViewModel => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = collageLayoutDisplayViewModel.Id.ToString(),
                        Text = collageLayoutDisplayViewModel.Name
                    }).ToArray(),
                    DefaultPhotoSessionContentCollageLayoutSelectListItems = enabledCollageLayoutViewModels.Where(collageLayoutDisplayViewModel => collageLayoutDisplayViewModel.PictureCount > 1).Select(collageLayoutDisplayViewModel => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = collageLayoutDisplayViewModel.Id.ToString(),
                        Text = collageLayoutDisplayViewModel.Name
                    }).ToArray(),
                    Albums = entity.Albums.Select((Album album) =>
                    {
                        var model = new AlbumDisplayViewModel();

                        model.Import(album);

                        return model;
                    }).ToList()
                };

                resultModel.Event.Import(entity);

                return View(resultModel);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [Route("add/album")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAlbum(AlbumInputViewModel modelInput)
        {
            Event @event = null;

            try
            {
                @event = Repository.Get(modelInput.EventId);
            }
            catch { }

            if (@event == null)
            {
                SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

                return GetDefaultActionResult();
            }

            if (@event.Albums.Any(entity => entity.Title == modelInput.Title))
            {
                ModelState.AddModelError("Title", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Title"]]);
            }

            if (modelInput.DefaultPictureContentCollageLayoutId == System.Guid.Empty)
            {
                ModelState.AddModelError("DefaultPictureContentCollageId", localizer["DefaultPictureContentCollageLayoutSelectionRequiredMessage"]);
            }

            if (modelInput.DefaultPhotoSessionContentCollageLayoutId == System.Guid.Empty)
            {
                ModelState.AddModelError("DefaultPhotoSessionContentCollageLayoutId", localizer["DefaultPhotoSessionContentCollageLayoutSelectionRequiredMessage"]);
            }

            if (ModelState.IsValid)
            {
                var album = modelInput.Export();

                album.DefaultPictureContentCollageLayout = collageLayoutRepository.Get(modelInput.DefaultPictureContentCollageLayoutId);

                album.DefaultPhotoSessionContentCollageLayout = collageLayoutRepository.Get(modelInput.DefaultPhotoSessionContentCollageLayoutId);

                @event.Albums.Add(album);

                Repository.Update(@event, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());
            }
            else
            {
                var messages = new List<string>();

                foreach (var item in ModelState)
                {
                    if (ModelState.GetFieldValidationState(item.Key) != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid) continue;

                    foreach (var error in item.Value.Errors)
                    {
                        messages.Add(error.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, messages.ToArray());
            }

            return RedirectToAction("albums", new { key = @event.Id });
        }

        [HttpGet]
        [Route("edit/{eventId}/album/{albumId}")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        public IActionResult AlbumEdit(System.Guid eventId, System.Guid albumId)
        {
            try
            {
                var @event = Repository.Get(eventId);

                var album = @event.Albums.First(e => e.Id == albumId);

                var enabledCollageLayoutViewModels = collageLayoutRepository.FindBy(collageLayout => collageLayout.Enabled).ToList().Select((CollageLayout collageLayout) =>
                {
                    var descriptor = Extend.Layout.Provision.Providers.CollageLayoutDescriptorProvider.Instance.GetCollageLayoutDescriptorById(collageLayout.Id);

                    var collageLayoutModel = new CollageLayoutDisplayViewModel();

                    collageLayoutModel.Import(collageLayout);

                    return collageLayoutModel;
                });

                var model = new AlbumInputViewModel
                {
                    EventId = @event.Id,
                    DefaultPictureContentCollageLayoutSelectListItems = enabledCollageLayoutViewModels.Where(collageLayoutDisplayViewModel => collageLayoutDisplayViewModel.PictureCount == 1).Select(collageLayoutDisplayViewModel => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = collageLayoutDisplayViewModel.Id.ToString(),
                        Text = collageLayoutDisplayViewModel.Name
                    }).ToArray(),
                    DefaultPhotoSessionContentCollageLayoutSelectListItems = enabledCollageLayoutViewModels.Where(collageLayoutDisplayViewModel => collageLayoutDisplayViewModel.PictureCount > 1).Select(collageLayoutDisplayViewModel => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = collageLayoutDisplayViewModel.Id.ToString(),
                        Text = collageLayoutDisplayViewModel.Name
                    }).ToArray(),
                };

                model.Import(album);

                return View(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            var notification = new NotificationViewModel
            {
                Type = NotificationType.Danger,
                Message = GetNotFoundMessage()
            };

            TempData[NotificationViewModel.DEFAULT_NOTIFICATION_MAP_KEY] = Newtonsoft.Json.JsonConvert.SerializeObject(notification);

            return GetDefaultActionResult();
        }

        [Route("edit/album")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAlbum(AlbumInputViewModel modelInput)
        {
            Event @event = null;

            try
            {
                @event = Repository.Get(modelInput.EventId);
            }
            catch { }

            if (@event == null)
            {
                SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

                return GetDefaultActionResult();
            }

            if (@event.Albums.Any(entity => entity.Id != modelInput.Id && entity.Title == modelInput.Title))
            {
                ModelState.AddModelError("Title", localizer["UniqueValueConvinationValidationErrorMessage", localizer["Title"]]);
            }

            if (modelInput.DefaultPictureContentCollageLayoutId == System.Guid.Empty)
            {
                ModelState.AddModelError("DefaultPictureContentCollageId", localizer["DefaultPictureContentCollageLayoutSelectionRequiredMessage"]);
            }

            if (modelInput.DefaultPhotoSessionContentCollageLayoutId == System.Guid.Empty)
            {
                ModelState.AddModelError("DefaultPhotoSessionContentCollageLayoutId", localizer["DefaultPhotoSessionContentCollageLayoutSelectionRequiredMessage"]);
            }

            if (ModelState.IsValid)
            {
                var album = @event.Albums.First(e => e.Id == modelInput.Id);

                modelInput.Mergue(album);

                if (album.DefaultPictureContentCollageLayout.Id != modelInput.DefaultPictureContentCollageLayoutId)
                {
                    album.DefaultPictureContentCollageLayout = collageLayoutRepository.Get(modelInput.DefaultPictureContentCollageLayoutId);
                }

                if (album.DefaultPhotoSessionContentCollageLayout.Id != modelInput.DefaultPhotoSessionContentCollageLayoutId)
                {
                    album.DefaultPhotoSessionContentCollageLayout = collageLayoutRepository.Get(modelInput.DefaultPhotoSessionContentCollageLayoutId);
                }

                albumRepository.Update(album, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());
            }
            else
            {
                var messages = new List<string>();

                foreach (var item in ModelState)
                {
                    if (ModelState.GetFieldValidationState(item.Key) != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid) continue;

                    foreach (var error in item.Value.Errors)
                    {
                        messages.Add(error.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, messages.ToArray());
            }

            return RedirectToAction("albums", new { key = @event.Id });
        }

        [Route("delete/album")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAlbum(AlbumDisplayViewModel modelDisplay)
        {
            Event @event = null;

            try
            {
                @event = Repository.Get(modelDisplay.Event.Id);
            }
            catch { }

            if (@event == null)
            {
                SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

                return GetDefaultActionResult();
            }

            try
            {
                var album = @event.Albums.First(e => e.Id == modelDisplay.Id);

                albumRepository.Delete(album, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            return RedirectToAction("albums", new { key = @event.Id });
        }

        [HttpGet]
        [Route("edit/{eventId}/album/{albumId}/locks")]
        public IActionResult EditAlbumLocks(System.Guid eventId, System.Guid albumId)
        {
            try
            {
                var @event = Repository.Get(eventId);

                var album = @event.Albums.First(e => e.Id == albumId);

                var result = new EventAlbumLocksViewModel
                {
                    Album = new AlbumDisplayViewModel(),
                    Locks = album.LockBridges.Select(bridge => new AlbumLockDisplayViewModel
                    {
                        Descriptor = Extend.Lock.Provision.Providers.AlbumLockDescriptorProvider.Instance.GetDescriptorById(bridge.LockId),
                        SerializedContext = bridge.SerializedContext
                    }).ToList()
                };

                result.Album.Import(album);

                result.Album.Event = new EventDisplayViewModel { Id = @event.Id };

                return View(result);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }
               
        [Route("add/album/lock/password")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAlbumPasswordLock(AlbumPasswordLockInputViewModel modelInput)
        {
            var @event = Repository.Get(modelInput.EventId);

            var album = @event.Albums.First(e => e.Id == modelInput.AlbumId);

            if (ModelState.IsValid)
            {
                album.LockBridges.Add(new AlbumLockBridge
                {
                    AlbumId = album.Id,
                    LockId = Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID,
                    SerializedContext = modelInput.Password
                });

                Repository.Update(@event, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());
            }
            else
            {
                var messages = new List<string>();

                foreach (var item in ModelState)
                {
                    if (ModelState.GetFieldValidationState(item.Key) != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid) continue;

                    foreach (var error in item.Value.Errors)
                    {
                        messages.Add(error.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, messages.ToArray());
            }

            return RedirectToAction("editalbumlocks", new { EventId = @event.Id, AlbumId = album.Id });
        }

        [Route("edit/album/lock/password")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAlbumPasswordLock(AlbumPasswordLockInputViewModel modelInput)
        {
            var @event = Repository.Get(modelInput.EventId);

            var album = @event.Albums.First(e => e.Id == modelInput.AlbumId);

            if (ModelState.IsValid)
            {
                var target = album.LockBridges.First(bridge => bridge.AlbumId == album.Id && bridge.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID);

                target.SerializedContext = modelInput.Password;

                albumRepository.Update(album, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());
            }
            else
            {
                var messages = new List<string>();

                foreach (var item in ModelState)
                {
                    if (ModelState.GetFieldValidationState(item.Key) != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid) continue;

                    foreach (var error in item.Value.Errors)
                    {
                        messages.Add(error.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, messages.ToArray());
            }

            return RedirectToAction("editalbumlocks", new { EventId = @event.Id, AlbumId = album.Id });
        }

        [Route("delete/album/lock/password")]
        [RequiresRoleBasePermission(Roles.EventEditor)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAlbumPasswordLock(AlbumPasswordLockInputViewModel modelInput)
        {
            var @event = Repository.Get(modelInput.EventId);

            var album = @event.Albums.First(e => e.Id == modelInput.AlbumId);

            try
            {
                var target = album.LockBridges.First(bridge => bridge.AlbumId == album.Id && bridge.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID);

                album.LockBridges.Remove(target);

                albumRepository.Update(album, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            return RedirectToAction("editalbumlocks", new { EventId = @event.Id, AlbumId = album.Id });
        }

        [HttpGet]
        [Route("edit/{eventKey}/albums/{albumKey}/contents/picture")]
        [RequiresRoleBasePermission(Roles.EventEditor, Roles.ContentViewer)]
        public IActionResult AlbumPictureContents(System.Guid eventKey, System.Guid albumKey)
        {
            try
            {
                var @event = Repository.Get(eventKey);

                var album = @event.Albums.First(e => e.Id == albumKey);

                var model = new EventAlbumPictureContentsViewModel
                {
                    Album = new AlbumDisplayViewModel
                    {
                        Event = new EventDisplayViewModel()
                    },
                    Contents = pictureContentRepository.FindBy(e => e.Album.Id == album.Id).ToList()
                };

                model.Album.Import(album);
                model.Album.Event.Import(@event);

                return View(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            var notification = new NotificationViewModel
            {
                Type = NotificationType.Danger,
                Message = GetNotFoundMessage()
            };

            TempData[NotificationViewModel.DEFAULT_NOTIFICATION_MAP_KEY] = Newtonsoft.Json.JsonConvert.SerializeObject(notification);

            return GetDefaultActionResult();
        }

        #endregion
    }
}
