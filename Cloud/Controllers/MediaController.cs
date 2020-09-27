using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;

namespace EM.Cloud.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme + ", " + Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("[area]/[controller]")]
    [Area("business")]
    public class MediaController : SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Thumbnail, System.Guid> thumbnailRepository;
        private readonly IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;

        public MediaController(IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Thumbnail, System.Guid> thumbnailRepository, IEntityRepository<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid> imageRepository, IImageFileManagerFacade imageFileManager, IThumbnailFileManagerFacade thumbnailFileManager, Microsoft.Extensions.Localization.IStringLocalizer<MediaController> localizer, ILogger<MediaController> logger)
            : base(localizer, logger)
        {
            this.thumbnailRepository = thumbnailRepository;
            this.imageRepository = imageRepository;
            this.imageFileManager = imageFileManager;
            this.thumbnailFileManager = thumbnailFileManager;
        }

        [RequiresRoleBasePermission(Roles.ContentViewer)]
        [HttpGet("[action]/{key}")]
        public IActionResult Image(System.Guid key)
        {
            try
            {
                var image = imageRepository.Get(key);

                var content = imageFileManager.GetImageFileContent(image);

                return new FileContentResult(content, image.ContentType);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NotFoundResult();
        }

        [RequiresRoleBasePermission(Roles.ContentViewer)]
        [HttpGet("[action]/{key}")]
        public IActionResult Thumbnail(System.Guid key)
        {
            try
            {
                var thumbnail = thumbnailRepository.Get(key);

                return new FileContentResult(thumbnailFileManager.GetThumbnailFileContent(thumbnail), thumbnail.ContentType);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NotFoundResult();
        }
    }
}
