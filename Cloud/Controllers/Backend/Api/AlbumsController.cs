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
using System.Linq;

namespace EM.Cloud.Controllers.Backend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/backend/[controller]")]
    [ApiController]
    public class AlbumsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<Album, System.Guid, AlbumInputModel, AlbumDisplayModel, Cloud.Backend.Api.Common.Models.Filter.AlbumFilterModel>
    {
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;
        private readonly IEntityRepository<CollageLayout, System.Guid> printLayoutRepository;
        private readonly IEntityRepository<Event, System.Guid> eventRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly Helpers.AlbumDeletedCascadeHelper albumDeletedCascadeHelper;

        public AlbumsController(Helpers.AlbumDeletedCascadeHelper albumDeletedCascadeHelper, Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions, IThumbnailFileManagerFacade thumbnailFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<Event, System.Guid> eventRepository, IEntityRepository<CollageLayout, System.Guid> printLayoutRepository, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Album, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<AlbumsController> localizer, ILogger<AlbumsController> logger) : base(repository, localizer, logger)
        {
            this.mediaThumbnailResizeOptions = mediaThumbnailResizeOptions;
            this.thumbnailFileManager = thumbnailFileManager;
            this.imageFileManager = imageFileManager;
            this.thumbnailRepository = thumbnailRepository;
            this.imageRepository = imageRepository;
            this.printLayoutRepository = printLayoutRepository;
            this.eventRepository = eventRepository;
            this.configuration = configuration;
            this.albumDeletedCascadeHelper = albumDeletedCascadeHelper;
        }

        [RequiresRoleBasePermission(Roles.AlbumCreator)]
        public override IActionResult Create([FromBody] AlbumInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        [RequiresRoleBasePermission(Roles.AlbumRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequiresRoleBasePermission(Roles.AlbumEditor)]
        public override IActionResult Edit([FromBody] AlbumInputModel inputModel)
        {
            return base.Edit(inputModel);
        }

        [RequiresRoleBasePermission(Roles.AlbumViewer)]
        public override IActionResult Get()
        {
            return base.Get();
        }

        [RequiresRoleBasePermission(Roles.AlbumViewer)]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        [RequiresRoleBasePermission(Roles.AlbumViewer)]
        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return base.GetPaginate(sheet, limit);
        }

        [RequiresRoleBasePermission(Roles.AlbumViewer)]
        public override IActionResult GetFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.AlbumFilterModel filterModel)
        {
            return base.GetFiltered(filterModel);
        }

        [RequiresRoleBasePermission(Roles.AlbumViewer)]
        public override IActionResult GetPaginateFiltered([FromBody] Cloud.Backend.Api.Common.Models.Filter.AlbumFilterModel filterModel, int sheet = 1, int limit = 25)
        {
            return base.GetPaginateFiltered(filterModel, sheet, limit);
        }

        protected override void PreCreate(Album entity, AlbumInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.Event = eventRepository.Get(inputModel.EventId);

            entity.DefaultPictureContentCollageLayout = printLayoutRepository.Get(inputModel.DefaultPictureContentCollageLayoutId);
            entity.DefaultPhotoSessionContentCollageLayout = printLayoutRepository.Get(inputModel.DefaultPhotoSessionContentCollageLayoutId);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

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
                            About = string.Format("Image for Overlay of Album: {0}", inputModel.Id)
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
                                    About = string.Format("Thumbnail of Image for Overlay of Album: {0}", inputModel.Id)
                                };

                                thumbnailFileManager.SaveThumbnailFileContent(entity.Overlay.Thumbnail.Id, thumbnailSkData.ToArray(), "image/jpeg");
                            }
                        }
                    }
                }
            }
        }

        protected override void PreEdit(Album entity, AlbumInputModel inputModel)
        {
            base.PreEdit(entity, inputModel);

            entity.DefaultPictureContentCollageLayout = printLayoutRepository.Get(inputModel.DefaultPictureContentCollageLayoutId);
            entity.DefaultPhotoSessionContentCollageLayout = printLayoutRepository.Get(inputModel.DefaultPhotoSessionContentCollageLayoutId);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

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
                                About = string.Format("Image for Overlay of Album: {0}", inputModel.Id)
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
                                        About = string.Format("Thumbnail of Image for Overlay of Album: {0}", inputModel.Id)
                                    };

                                    thumbnailFileManager.SaveThumbnailFileContent(entity.Overlay.Thumbnail.Id, thumbnailSkData.ToArray(), "image/jpeg");
                                }
                            }
                        }
                    }
                }

                var oldThumbnail = oldImage.Thumbnail;

                thumbnailRepository.Delete(oldThumbnail, User.Identity.Name);

                thumbnailFileManager.DeleteThumbnailFile(oldThumbnail);

                imageRepository.Delete(oldImage, User.Identity.Name);

                imageFileManager.DeleteImageFile(oldImage);
            }
        }

        protected override void PostDelete(Album entity)
        {
            base.PostDelete(entity);

            albumDeletedCascadeHelper.Cascade(entity, User.Identity, true, false, true);
        }

        #region Locks management enpoints

        [RequiresRoleBasePermission(Roles.AlbumViewer)]
        [HttpGet("{albumId}/locks")]
        public ActionResult<AlbumLockDisplayModel[]> GetLocks(System.Guid albumId)
        {
            try
            {
                var album = Repository.Get(albumId);

                return album.LockBridges.Select(bridge => new AlbumLockDisplayModel
                {
                    LockId = bridge.LockId,
                    SerializedContext = bridge.SerializedContext
                }).ToArray();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            return NoContent();
        }

        [RequiresRoleBasePermission(Roles.AlbumEditor)]
        [HttpPost("lock")]
        public IActionResult AddAlbumLock([FromBody] AlbumLockInputModel modelInput)
        {
            try
            {
                var album = Repository.Get(modelInput.AlbumId);

                if (ModelState.IsValid)
                {
                    album.LockBridges.Add(new AlbumLockBridge
                    {
                        AlbumId = album.Id,
                        LockId = modelInput.LockId,
                        SerializedContext = modelInput.SerializedContext
                    });

                    Repository.Update(album, User.Identity.Name);

                    return Ok();
                }

                return BadRequest();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            return NoContent();
        }

        [RequiresRoleBasePermission(Roles.AlbumEditor)]
        [HttpPut("lock")]
        public IActionResult EditAlbumLock([FromBody] AlbumLockInputModel modelInput)
        {
            try
            {
                var album = Repository.Get(modelInput.AlbumId);

                if (ModelState.IsValid)
                {
                    var target = album.LockBridges.First(bridge => bridge.AlbumId == album.Id && bridge.LockId == modelInput.LockId);

                    target.SerializedContext = modelInput.SerializedContext;

                    Repository.Update(album, User.Identity.Name);

                    return Ok();
                }

                return BadRequest();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            return NoContent();
        }

        [RequiresRoleBasePermission(Roles.AlbumRemover)]
        [HttpDelete("{albumId}/locks/{lockId}")]
        public IActionResult DeleteAlbumLock(System.Guid albumId, System.Guid lockId)
        {
            var album = Repository.Get(albumId);

            try
            {
                var target = album.LockBridges.First(bridge => bridge.AlbumId == album.Id && bridge.LockId == lockId);

                album.LockBridges.Remove(target);

                Repository.Update(album, User.Identity.Name);

                return Ok();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            return NoContent();
        }

        #endregion
    }
}
