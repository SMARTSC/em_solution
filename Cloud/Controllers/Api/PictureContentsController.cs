using EM.Cloud.Api.Common.Models;
using EM.Cloud.Extensions;
using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;

namespace EM.Cloud.Controllers.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PictureContentsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementController<PictureContent, System.Guid, PictureContentInputModel, PictureContentDisplayModel>
    {
        private readonly IEntityRepository<Album, System.Guid> albumRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Thumbnail, System.Guid> thumbnailRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions;

        public PictureContentsController(Microsoft.Extensions.Options.IOptions<Options.MediaThumbnailResizeOptions> mediaThumbnailResizeOptions, IThumbnailFileManagerFacade thumbnailFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<Thumbnail, System.Guid> thumbnailRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Album, System.Guid> albumRepository, IEntityRepository<PictureContent, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<PictureContentsController> localizer, ILogger<PictureContentsController> logger) : base(repository, localizer, logger)
        {
            this.mediaThumbnailResizeOptions = mediaThumbnailResizeOptions;
            this.thumbnailFileManager = thumbnailFileManager;
            this.imageFileManager = imageFileManager;
            this.thumbnailRepository = thumbnailRepository;
            this.imageRepository = imageRepository;
            this.albumRepository = albumRepository;
        }

        public override IActionResult Get()
        {
            return NotFound();
        }

        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            return NotFound();
        }

        [RequiresRoleBasePermission(Roles.ContentViewer)]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        [RequiresRoleBasePermission(Roles.ContentCreator)]
        public override IActionResult Create([FromBody] PictureContentInputModel inputModel)
        {
            return base.Create(inputModel);
        }

        public override IActionResult Edit([FromBody] PictureContentInputModel inputModel)
        {
            return new ConflictResult();
        }

        [RequiresRoleBasePermission(Roles.ContentRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        protected override void PreCreate(PictureContent entity, PictureContentInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            entity.Album = albumRepository.Get(inputModel.AlbumId);

            using (var imageMemoryStream = new System.IO.MemoryStream(inputModel.ContentData))
            {
                using (var imageSkData = SkiaSharp.SKData.Create(imageMemoryStream))
                {
                    using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
                    {
                        entity.Image = new Image
                        {
                            ContentType = inputModel.ContentType,
                            Length = inputModel.ContentData.LongLength,
                            Width = imageSkBitmap.Width,
                            Height = imageSkBitmap.Height,
                            About = string.Format("Image for Picture Content :{0}", inputModel.Id)
                        };

                        imageFileManager.SaveImageFileContent(entity.Image.Id, inputModel.ContentData, inputModel.ContentType);

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

                                entity.Image.Thumbnail = new Thumbnail
                                {
                                    ContentType = inputModel.ContentType,
                                    Length = thumbnailSkData.Size,
                                    About = string.Format("Thumbnail of Image for Picture Content :{0}", inputModel.Id)
                                };

                                thumbnailFileManager.SaveThumbnailFileContent(entity.Image.Thumbnail.Id, thumbnailSkData.ToArray(), "image/jpeg");
                            }
                        }
                    }
                }
            }
        }

        protected override void PostDelete(PictureContent entity)
        {
            base.PostDelete(entity);

            thumbnailRepository.Delete(entity.Image.Thumbnail, User.Identity.Name);

            thumbnailFileManager.DeleteThumbnailFile(entity.Image.Thumbnail);

            imageRepository.Delete(entity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Image);
        }
    }
}
