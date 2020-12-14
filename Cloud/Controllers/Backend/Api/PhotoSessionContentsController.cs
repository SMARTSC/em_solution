using EM.Cloud.Api.Common.Models;
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
    public class PhotoSessionContentsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<PhotoSessionContent, System.Guid, PhotoSessionContentInputModel, PhotoSessionContentDisplayModel, Cloud.Backend.Api.Common.Models.Filter.PhotoSessionContentFilterModel>
    {
        private readonly IEntityRepository<Album, System.Guid> albumRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public PhotoSessionContentsController(Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions, IThumbnailFileManagerFacade thumbnailFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Album, System.Guid> albumRepository, IEntityRepository<PhotoSessionContent, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<PhotoSessionContentsController> localizer, ILogger<PhotoSessionContentsController> logger) : base(repository, localizer, logger)
        {
            this.mediaThumbnailResizeOptions = mediaThumbnailResizeOptions;
            this.thumbnailFileManager = thumbnailFileManager;
            this.imageFileManager = imageFileManager;
            this.thumbnailRepository = thumbnailRepository;
            this.imageRepository = imageRepository;
            this.albumRepository = albumRepository;
            this.configuration = configuration;
        }

        public override IActionResult Get()
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.ContentViewer)]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return NotFound();
        }

        public override IActionResult GetFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.PhotoSessionContentFilterModel filterModel)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.ContentViewer)]
        public override IActionResult GetPaginateFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.PhotoSessionContentFilterModel filterModel, int sheet = 1, int limit = 25)
        {
            return base.GetPaginateFiltered(filterModel, sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.ContentCreator)]
        public override IActionResult Create([FromBody] PhotoSessionContentInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        public override IActionResult Edit([FromBody] PhotoSessionContentInputModel inputModel)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.ContentEditor)]
        [HttpPut("{key}")]
        public IActionResult Agregate(System.Guid key, [FromBody] ImageMediaInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = Repository.Get(key);

                    var bridge = new PhotoSessionContentImageBridge
                    {
                        PhotoSessionId = entity.Id,
                        PhotoSession = entity
                    };

                    using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.ContentData))
                    {
                        using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                        {
                            using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                            {
                                bridge.Image = new Image
                                {
                                    ContentType = inputModel.ContentType,
                                    Length = inputModel.ContentData.LongLength,
                                    Width = imageSkBitmap.Width,
                                    Height = imageSkBitmap.Height,
                                    About = string.Format("Image for Photo Session Content :{0}", entity.Id)
                                };

                                bridge.ImageId = bridge.Image.Id;

                                Providers.ImagesBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Album.Id);
                                Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Album.Id); imageFileManager.SaveImageFileContent(bridge.Image.Id, inputModel.ContentData, inputModel.ContentType);

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

                                        bridge.Image.Thumbnail = new Thumbnail
                                        {
                                            ContentType = inputModel.ContentType,
                                            Length = thumbnailSkData.Size,
                                            About = string.Format("Thumbnail of Image for Photo Session Content :{0}", entity.Id)
                                        };

                                        thumbnailFileManager.SaveThumbnailFileContent(bridge.Image.Thumbnail.Id, thumbnailSkData.ToArray(), "image/jpeg");
                                    }
                                }
                            }
                        }
                    }

                    entity.ImageBridges.Add(bridge);

                    Repository.Update(entity, User.Identity.Name);

                    return Ok();
                }
                catch (System.InvalidOperationException e)
                {
                    logger.LogError(e, e.Message, null);

                    return NoContent();
                }
                catch (System.Exception e)
                {
                    logger.LogError(e, e.Message, null);

                    return Conflict();
                }
            }

            return BadRequest();
        }

        [RequiresRoleBasePermission(Roles.ContentRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        protected override void PreCreate(PhotoSessionContent entity, PhotoSessionContentInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.Album = albumRepository.Get(inputModel.AlbumId);
        }

        protected override void PostDelete(PhotoSessionContent entity)
        {
            base.PostDelete(entity);

            foreach (var bridge in entity.ImageBridges)
            {
                Providers.ImagesBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Album.Id);
                Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Album.Id); 
                
                thumbnailRepository.Delete(bridge.Image.Thumbnail, User.Identity.Name);

                thumbnailFileManager.DeleteThumbnailFile(bridge.Image.Thumbnail);

                imageRepository.Delete(bridge.Image, User.Identity.Name);

                imageFileManager.DeleteImageFile(bridge.Image);
            }
        }
    }
}
