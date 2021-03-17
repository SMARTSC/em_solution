using EM.Cloud.Backend.Api.Common.Models;
using EM.Cloud.Extensions;
using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;

namespace EM.Cloud.Controllers.Backend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/backend/[controller]")]
    [ApiController]
    public class EventsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<Event, System.Guid, EventInputModel, EventDisplayModel, Cloud.Backend.Api.Common.Models.Filter.EventFilterModel>
    {
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly Helpers.AlbumDeletedCascadeHelper albumDeletedCascadeHelper;

        public EventsController(Helpers.AlbumDeletedCascadeHelper albumDeletedCascadeHelper, Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions, IThumbnailFileManagerFacade thumbnailFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Event, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<EventsController> localizer, ILogger<EventsController> logger) : base(repository, localizer, logger)
        {
            this.mediaThumbnailResizeOptions = mediaThumbnailResizeOptions;
            this.thumbnailFileManager = thumbnailFileManager;
            this.imageFileManager = imageFileManager;
            this.thumbnailRepository = thumbnailRepository;
            this.imageRepository = imageRepository;
            this.configuration = configuration;
            this.albumDeletedCascadeHelper = albumDeletedCascadeHelper;
        }

        [RequiresRoleBasePermission(Roles.EventCreator)]
        public override IActionResult Create([FromBody] EventInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        [RequiresRoleBasePermission(Roles.EventRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequiresRoleBasePermission(Roles.EventEditor)]
        public override IActionResult Edit([FromBody] EventInputModel inputModel)
        {
            return base.Edit(inputModel);
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult Get()
        {
            return base.Get();
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return base.GetPaginate(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.EventViewer)]
        public override IActionResult GetFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.EventFilterModel filterModel)
        {
            return base.GetFiltered(filterModel);
        }

        protected override void PreCreate(Event entity, EventInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.Logo.ContentData))
            {
                using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                {
                    using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                    {
                        entity.Logo = new Image
                        {
                            ContentType = inputModel.Logo.ContentType,
                            Length = inputModel.Logo.ContentData.LongLength,
                            Width = imageSkBitmap.Width,
                            Height = imageSkBitmap.Height,
                            About = string.Format("Image for Logo of Event: {0}", inputModel.Id)
                        };

                        imageFileManager.SaveImageFileContent(entity.Logo.Id, inputModel.Logo.ContentData, inputModel.Logo.ContentType);
                    }
                }
            }

            using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.Overlay.ContentData))
            {
                using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                {
                    using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                    {
                        entity.Overlay = new Image
                        {
                            ContentType = inputModel.Overlay.ContentType,
                            Length = inputModel.Overlay.ContentData.LongLength,
                            Width = imageSkBitmap.Width,
                            Height = imageSkBitmap.Height,
                            About = string.Format("Image for Overlay of Event: {0}", inputModel.Id)
                        };

                        imageFileManager.SaveImageFileContent(entity.Overlay.Id, inputModel.Overlay.ContentData, inputModel.Overlay.ContentType);

                        SkiaSharp.SKImageInfo desiredImageInfo;

                        if (imageSkBitmap.Width > imageSkBitmap.Height)
                        {
                            desiredImageInfo = imageSkBitmap.Info.LandscapeScale(mediaThumbnailResizeOptions.Value.DesiredWidth);
                        }
                        else
                        {
                            desiredImageInfo = imageSkBitmap.Info.PortraitScale(mediaThumbnailResizeOptions.Value.DesiredHeigth);
                        }

                        using (var thumbnailSkBitmap = imageSkBitmap.Resize(desiredImageInfo, SkiaSharp.SKFilterQuality.High))
                        {
                            using (var thumbnailSkImage = SkiaSharp.SKImage.FromBitmap(thumbnailSkBitmap))
                            {
                                var thumbnailSkData = thumbnailSkImage.Encode(SkiaSharp.SKEncodedImageFormat.Jpeg, 75);

                                entity.Overlay.Thumbnail = new Thumbnail
                                {
                                    ContentType = inputModel.Overlay.ContentType,
                                    Length = thumbnailSkData.Size,
                                    About = string.Format("Thumbnail of Image for Overlay of Event: {0}", inputModel.Id)
                                };

                                thumbnailFileManager.SaveThumbnailFileContent(entity.Overlay.Thumbnail.Id, thumbnailSkData.ToArray(), "image/jpeg");
                            }
                        }
                    }
                }
            }
        }

        protected override void PreEdit(Event entity, EventInputModel inputModel)
        {
            base.PreEdit(entity, inputModel);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            if (inputModel.Logo != null)
            {
                var oldImage = entity.Logo;

                using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.Logo.ContentData))
                {
                    using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                    {
                        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                        {
                            entity.Logo = new Image
                            {
                                ContentType = inputModel.Logo.ContentType,
                                Length = inputModel.Logo.ContentData.LongLength,
                                Width = imageSkBitmap.Width,
                                Height = imageSkBitmap.Height,
                                About = string.Format("Image for Logo of Event: {0}", inputModel.Id)
                            };

                            imageFileManager.SaveImageFileContent(entity.Logo.Id, inputModel.Logo.ContentData, inputModel.Logo.ContentType);
                        }
                    }
                }

                imageRepository.Delete(oldImage, User.Identity.Name);

                imageFileManager.DeleteImageFile(oldImage);
            }

            if (inputModel.Overlay != null)
            {
                var oldImage = entity.Overlay;

                using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.Overlay.ContentData))
                {
                    using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                    {
                        using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                        {
                            entity.Overlay = new Image
                            {
                                ContentType = inputModel.Overlay.ContentType,
                                Length = inputModel.Overlay.ContentData.LongLength,
                                Width = imageSkBitmap.Width,
                                Height = imageSkBitmap.Height,
                                About = string.Format("Image for Overlay of Event: {0}", inputModel.Id)
                            };

                            imageFileManager.SaveImageFileContent(entity.Overlay.Id, inputModel.Overlay.ContentData, inputModel.Overlay.ContentType);

                            SkiaSharp.SKImageInfo desiredImageInfo;

                            if (imageSkBitmap.Width > imageSkBitmap.Height)
                            {
                                desiredImageInfo = imageSkBitmap.Info.LandscapeScale(mediaThumbnailResizeOptions.Value.DesiredWidth);
                            }
                            else
                            {
                                desiredImageInfo = imageSkBitmap.Info.PortraitScale(mediaThumbnailResizeOptions.Value.DesiredHeigth);
                            }

                            using (var thumbnailSkBitmap = imageSkBitmap.Resize(desiredImageInfo, SkiaSharp.SKFilterQuality.High))
                            {
                                using (var thumbnailSkImage = SkiaSharp.SKImage.FromBitmap(thumbnailSkBitmap))
                                {
                                    var thumbnailSkData = thumbnailSkImage.Encode(SkiaSharp.SKEncodedImageFormat.Jpeg, 75);

                                    entity.Overlay.Thumbnail = new Thumbnail
                                    {
                                        ContentType = inputModel.Overlay.ContentType,
                                        Length = thumbnailSkData.Size,
                                        About = string.Format("Thumbnail of Image for Overlay of Event: {0}", inputModel.Id)
                                    };

                                    thumbnailFileManager.SaveThumbnailFileContent(entity.Overlay.Thumbnail.Id, thumbnailSkData.ToArray(), "image/jpeg");
                                }
                            }
                        }
                    }
                }

                var oldThumbnail = oldImage.Thumbnail;

                thumbnailRepository.Delete(oldThumbnail, User.Identity.Name, false);

                thumbnailFileManager.DeleteThumbnailFile(oldThumbnail);

                imageRepository.Delete(oldImage, User.Identity.Name);

                imageFileManager.DeleteImageFile(oldImage);
            }
        }

        protected override void PreDelete(Event entity)
        {
            base.PreDelete(entity);

            //var index = 0;
            //var endIndex = entity.Albums.Count - 1;

            foreach (var item in entity.Albums)
            {
                albumDeletedCascadeHelper.Cascade(item, User.Identity, false, true, false);

                //index++;
            }
        }

        protected override void PostDelete(Event entity)
        {
            base.PostDelete(entity);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            imageRepository.Delete(entity.Logo, User.Identity.Name, false);

            imageFileManager.DeleteImageFile(entity.Logo);

            var overlayThumbnail = entity.Overlay.Thumbnail;

            thumbnailRepository.Delete(overlayThumbnail, User.Identity.Name, false);

            thumbnailFileManager.DeleteThumbnailFile(overlayThumbnail);

            imageRepository.Delete(entity.Overlay, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Overlay);
        }
    }
}
