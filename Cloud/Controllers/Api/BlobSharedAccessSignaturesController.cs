using EM.Cloud.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace EM.Cloud.Controllers.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BlobSharedAccessSignaturesController : Controller
    {
        private readonly Microsoft.Extensions.Options.IOptions<Options.SharedAccessSignatureOptions> sharedAccessSignatureOptions;
        private readonly IConfiguration configuration;
        private readonly IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository;
        private readonly IEntityRepository<Persistence.Entities.Event, System.Guid> eventRepository;
        private readonly IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository;

        public BlobSharedAccessSignaturesController(IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository, IEntityRepository<Persistence.Entities.Event, System.Guid> eventRepository, IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository, IConfiguration configuration, Microsoft.Extensions.Options.IOptions<Options.SharedAccessSignatureOptions> sharedAccessSignatureOptions)
        {
            this.sharedAccessSignatureOptions = sharedAccessSignatureOptions;
            this.configuration = configuration;
            this.albumRepository = albumRepository;
            this.eventRepository = eventRepository;
            this.terminalRepository = terminalRepository;
        }

        [HttpGet("album/{key}/[action]")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos de Acceso a Contenidos al Album", typeof(Cloud.Api.Common.Models.AlbumContentsBlobSharedAccessSignatureModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "No autorizado a acceder al Album!")]
        public IActionResult Contents(System.Guid key)
        {
            var terminal = terminalRepository.Get(User.Identity.Name);

            if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == key))
            {
                return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
            }

            var entity = albumRepository.Get(key);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Id);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Id);

            var expiresOn = System.DateTimeOffset.UtcNow.AddHours(sharedAccessSignatureOptions.Value.AlbumBlobContainerTokensExpirationHourCount);

            return Ok(new Cloud.Api.Common.Models.AlbumContentsBlobSharedAccessSignatureModel
            {
                ImagesBlobContainerUri = Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient().GenerateSasUri(Azure.Storage.Sas.BlobContainerSasPermissions.List | Azure.Storage.Sas.BlobContainerSasPermissions.Read, expiresOn),
                ThumbnailsBlobContainerUri = Providers.ThumbnailsBlobContainerClientProvider.Instance.GetBlobContainerClient().GenerateSasUri(Azure.Storage.Sas.BlobContainerSasPermissions.List | Azure.Storage.Sas.BlobContainerSasPermissions.Read, expiresOn),
                ExpiresOn = expiresOn
            });
        }

        [HttpGet("[action]/{key}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos de Acceso al Album", typeof(Cloud.Api.Common.Models.AlbumBlobSharedAccessSignatureModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "No autorizado a acceder al Album!")]
        public IActionResult Album(System.Guid key)
        {
            var terminal = terminalRepository.Get(User.Identity.Name);

            if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == key))
            {
                return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
            }

            var entity = albumRepository.Get(key);

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            var expiresOn = System.DateTimeOffset.UtcNow.AddHours(1);

            var overlayImageBlobClient = new Azure.Storage.Blobs.BlobClient(configuration.GetConnectionString("BlobStorageConnection"), "media-images", entity.Overlay.Id.ToBlobId(entity.Overlay.ContentType));

            if (!overlayImageBlobClient.CanGenerateSasUri)
            {
                return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
            }

            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            var overlayThumbnailBlobClient = new Azure.Storage.Blobs.BlobClient(configuration.GetConnectionString("BlobStorageConnection"), "media-thumbnails", entity.Overlay.Thumbnail.Id.ToBlobId(entity.Overlay.Thumbnail.ContentType));

            return Ok(new Cloud.Api.Common.Models.AlbumBlobSharedAccessSignatureModel
            {
                OverlayImageBlobUri = overlayImageBlobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.List | Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn),
                OverlayThumbnailBlobUri = overlayThumbnailBlobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.List | Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn)
            });
        }

        [HttpGet("[action]/{key}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos de Acceso al Logo del Eveto", typeof(Cloud.Api.Common.Models.EventBlobSharedAccessSignatureModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "No es posible generar token SAS!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Evento no encontrado!")]
        public IActionResult Event(System.Guid key)
        {
            try
            {
                var entity = eventRepository.Get(key);

                Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

                var expiresOn = System.DateTimeOffset.UtcNow.AddHours(1);

                var blobClient = new Azure.Storage.Blobs.BlobClient(configuration.GetConnectionString("BlobStorageConnection"), "media-images", entity.Logo.Id.ToBlobId(entity.Logo.ContentType));

                if (!blobClient.CanGenerateSasUri)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                var overlayImageBlobClient = new Azure.Storage.Blobs.BlobClient(configuration.GetConnectionString("BlobStorageConnection"), "media-images", entity.Overlay.Id.ToBlobId(entity.Overlay.ContentType));

                if (!overlayImageBlobClient.CanGenerateSasUri)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

                var overlayThumbnailBlobClient = new Azure.Storage.Blobs.BlobClient(configuration.GetConnectionString("BlobStorageConnection"), "media-thumbnails", entity.Overlay.Thumbnail.Id.ToBlobId(entity.Overlay.Thumbnail.ContentType));

                return Ok(new Cloud.Api.Common.Models.EventBlobSharedAccessSignatureModel
                {
                    LogoImageBlobUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.List | Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn),
                    OverlayImageBlobUri = overlayImageBlobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.List | Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn),
                    OverlayThumbnailBlobUri = overlayThumbnailBlobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.List | Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn)
                });
            }
            catch
            {
                return NoContent();
            }
        }
    }
}