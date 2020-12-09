using EM.Cloud.Persistence.Entities;
using EM.Cloud.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Filters;
using SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade;
using System.Linq;

namespace EM.Cloud.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme + ", " + Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("[area]/[controller]")]
    [Area("business")]
    public class ImageController : SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly IEntityRepository<Event, System.Guid> eventRepository;
        private readonly IEntityRepository<Client, System.Guid> clientRepository;
        private readonly IEntityRepository<Album, System.Guid> albumRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IThumbnailFileManagerFacade thumbnailFileManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public ImageController(Microsoft.Extensions.Configuration.IConfiguration configuration, IEntityRepository<Album, System.Guid> albumRepository, IEntityRepository<Event, System.Guid> eventRepository, IEntityRepository<Client, System.Guid> clientRepository, IImageFileManagerFacade imageFileManager, IThumbnailFileManagerFacade thumbnailFileManager, Microsoft.Extensions.Localization.IStringLocalizer<ImageController> localizer, ILogger<ImageController> logger)
            : base(localizer, logger)
        {
            this.eventRepository = eventRepository;
            this.clientRepository = clientRepository;
            this.albumRepository = albumRepository;
            this.imageFileManager = imageFileManager;
            this.thumbnailFileManager = thumbnailFileManager;
            this.configuration = configuration;
        }

        [ResponseCache(NoStore = true)]
        [RequiresRoleBasePermission(Roles.ClientViewer)]
        [HttpGet("[action]/{key}")]
        public IActionResult Client(System.Guid key)
        {
            try
            {
                var client = clientRepository.Get(key);

                var image = client is PersonClient ? (client as PersonClient).Identity.Image : (client as OrganizationClient).Identity.Image;

                if (image == null)
                {
                    return File("img/default-person.png", "image/png");
                }

                Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

                return new FileContentResult(imageFileManager.GetImageFileContent(image), image.ContentType);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NotFoundResult();
        }

        [ResponseCache(NoStore = true)]
        [RequiresRoleBasePermission(Roles.EventViewer)]
        [HttpGet("event/{key}/logo")]
        public IActionResult EventLogo(System.Guid key)
        {
            try
            {
                var @event = eventRepository.Get(key);

                Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

                return new FileContentResult(imageFileManager.GetImageFileContent(@event.Logo), @event.Logo.ContentType);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NotFoundResult();
        }

        [ResponseCache(NoStore = true)]
        [RequiresRoleBasePermission(Roles.EventViewer)]
        [HttpGet("album/{albumId}/qr")]
        [HttpGet("album/{albumId}/lock/{lockId}/qr")]
        public IActionResult AlbumLockQr(System.Guid albumId, System.Guid lockId = default)
        {
            try
            {
                var album = albumRepository.Get(albumId);

                if (lockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID)
                {
                    var context = album.LockBridges.First(bridge => bridge.LockId == lockId).SerializedContext;

                    return new FileContentResult(GetQrCodeImageContent(string.Format("em-album-pw://{0}:{1}", album.Alias, context)), "image/png");
                }

                if (album.LockBridges.Count > 0)
                {
                    return Forbid();
                }

                return new FileContentResult(GetQrCodeImageContent(string.Format("em-album://{0}", album.Alias)), "image/png");
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                logger.LogError(e.Message, e.InnerException);
            }
            catch { }

            return new NotFoundResult();
        }

        //[ResponseCache(NoStore = true)]
        //[RequiresRoleBasePermission(Roles.EventViewer)]
        //[HttpGet("event/{key}/logo/print/middle")]
        //public IActionResult EventMiddlePrintLogo(System.Guid key)
        //{
        //    try
        //    {
        //        var @event = eventRepository.Get(key);

        //        return new FileContentResult(imageFileManager.GetImageFileContent(@event.PrintMiddleLogo), @event.PrintMiddleLogo.ContentType);
        //    }
        //    catch (System.IO.DirectoryNotFoundException e)
        //    {
        //        logger.LogError(e.Message, e.InnerException);
        //    }
        //    catch { }

        //    return new NotFoundResult();
        //}

        //[ResponseCache(NoStore = true)]
        //[RequiresRoleBasePermission(Roles.EventViewer)]
        //[HttpGet("event/{key}/logo/print/middle/strech")]
        //public IActionResult EventStrechMiddlePrintLogo(System.Guid key)
        //{
        //    try
        //    {
        //        var @event = eventRepository.Get(key);

        //        return new FileContentResult(imageFileManager.GetImageFileContent(@event.PrintMiddleStrechLogo), @event.PrintMiddleStrechLogo.ContentType);
        //    }
        //    catch (System.IO.DirectoryNotFoundException e)
        //    {
        //        logger.LogError(e.Message, e.InnerException);
        //    }
        //    catch { }

        //    return new NotFoundResult();
        //}

        //[ResponseCache(NoStore = true)]
        //[RequiresRoleBasePermission(Roles.EventViewer)]
        //[HttpGet("event/{key}/logo/print/small")]
        //public IActionResult EventSmallPrintLogo(System.Guid key)
        //{
        //    try
        //    {
        //        var @event = eventRepository.Get(key);

        //        return new FileContentResult(imageFileManager.GetImageFileContent(@event.PrintSmallLogo), @event.PrintSmallLogo.ContentType);
        //    }
        //    catch (System.IO.DirectoryNotFoundException e)
        //    {
        //        logger.LogError(e.Message, e.InnerException);
        //    }
        //    catch { }

        //    return new NotFoundResult();
        //}

        //[ResponseCache(NoStore = true)]
        //[RequiresRoleBasePermission(Roles.EventViewer)]
        //[HttpGet("event/{key}/logo/print/small/strech")]
        //public IActionResult EventStrechSmallPrintLogo(System.Guid key)
        //{
        //    try
        //    {
        //        var @event = eventRepository.Get(key);

        //        return new FileContentResult(imageFileManager.GetImageFileContent(@event.PrintSmallStrechLogo), @event.PrintSmallStrechLogo.ContentType);
        //    }
        //    catch (System.IO.DirectoryNotFoundException e)
        //    {
        //        logger.LogError(e.Message, e.InnerException);
        //    }
        //    catch { }

        //    return new NotFoundResult();
        //}

        //[ResponseCache(NoStore = true)]
        //[RequiresRoleBasePermission(Roles.EventViewer)]
        //[HttpGet("[action]/{key}/resize/{orientation}/{dimention}")]
        //public IActionResult Event(System.Guid key, PBSP.Api.Common.Enumerations.ResizeOrientation orientation, int dimention)
        //{
        //    try
        //    {
        //        var @event = eventRepository.Get(key);

        //        if (@event.Logo == null)
        //        {
        //            return File("img/default-event.png", "image/png");
        //        }

        //        byte[] resultContent = null;

        //        using (var imageStream = new System.IO.FileStream(imageFileManager.GetImageAbsoluteFilePath(@event.Logo), System.IO.FileMode.Open))
        //        {
        //            using (var imageSkData = SkiaSharp.SKData.Create(imageStream))
        //            {
        //                using (var imageSkBitmap = SkiaSharp.SKBitmap.Decode(imageSkData))
        //                {
        //                    SkiaSharp.SKImageInfo desiredImageInfo;

        //                    if (orientation == PBSP.Api.Common.Enumerations.ResizeOrientation.Landscape)
        //                    {
        //                        desiredImageInfo = imageSkBitmap.Info.LandscapeScale(dimention);
        //                    }
        //                    else
        //                    {
        //                        desiredImageInfo = imageSkBitmap.Info.PortraitScale(dimention);
        //                    }
        //                    using (var thumbnailSkBitmap = imageSkBitmap.Resize(desiredImageInfo, SkiaSharp.SKFilterQuality.High))
        //                    {
        //                        using (var thumbnailSkImage = SkiaSharp.SKImage.FromBitmap(thumbnailSkBitmap))
        //                        {
        //                            var thumbnailSkData = thumbnailSkImage.Encode(SkiaSharp.SKEncodedImageFormat.Jpeg, 75);

        //                            resultContent = thumbnailSkData.ToArray();
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return new FileContentResult(resultContent, @event.Logo.ContentType);
        //    }
        //    catch (System.IO.DirectoryNotFoundException e)
        //    {
        //        logger.LogError(e.Message, e.InnerException);
        //    }
        //    catch { }

        //    return new NotFoundResult();
        //}

        private byte[] GetQrCodeImageContent(string action)
        {
            byte[] result;

            var qrGenerator = new QRCoder.QRCodeGenerator();

            using (var qrCodeData = qrGenerator.CreateQrCode(action, QRCoder.QRCodeGenerator.ECCLevel.Q))
            {
                using (var qrCode = new QRCoder.QRCode(qrCodeData))
                {
                    using (var qrCodeImage = qrCode.GetGraphic(10))
                    {
                        using (var stream = new System.IO.MemoryStream())
                        {
                            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                            result = stream.ToArray();
                        }
                    }
                }
            };

            return result;
        }
    }
}
