using EM.Cloud.Extensions;
using Microsoft.AspNetCore.Mvc;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace EM.Cloud.Controllers.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BlobSharedAccessSignaturesController : Controller
    {
        private readonly Microsoft.Extensions.Options.IOptions<Options.SharedAccessSignatureOptions> sharedAccessSignatureOptions;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository;
        private readonly IEntityRepository<Persistence.Entities.Event, System.Guid> eventRepository;

        public BlobSharedAccessSignaturesController(IEntityRepository<Persistence.Entities.Event, System.Guid> eventRepository, IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository, Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.Options.IOptions<Options.SharedAccessSignatureOptions> sharedAccessSignatureOptions)
        {
            this.sharedAccessSignatureOptions = sharedAccessSignatureOptions;
            this.configuration = configuration;
            this.albumRepository = albumRepository;
            this.eventRepository = eventRepository;
        }

        [HttpGet("[action]/{key}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos de Acceso al Album", typeof(Cloud.Api.Common.Models.AlbumBlobSharedAccessSignatureModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "No autorizado a acceder al Album!")]
        public IActionResult Album(System.Guid key)
        {
            var entity = albumRepository.Get(key);

            // TODO Assert album accesibility!

            Providers.ImagesBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Id);
            Providers.ThumbnailsBlobContainerClientProvider.Instance.TargetAlbumBlobContainer(configuration, entity.Id);

            var expiresOn = System.DateTimeOffset.UtcNow.AddHours(sharedAccessSignatureOptions.Value.AlbumBlobContainerTokensExpirationHourCount);

            return Ok(new Cloud.Api.Common.Models.AlbumBlobSharedAccessSignatureModel
            {
                ImagesBlobContainerUri = Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient().GenerateSasUri(Azure.Storage.Sas.BlobContainerSasPermissions.List | Azure.Storage.Sas.BlobContainerSasPermissions.Read, expiresOn),
                ThumbnailsBlobContainerUri = Providers.ThumbnailsBlobContainerClientProvider.Instance.GetBlobContainerClient().GenerateSasUri(Azure.Storage.Sas.BlobContainerSasPermissions.List | Azure.Storage.Sas.BlobContainerSasPermissions.Read, expiresOn),
                ExpiresOn = expiresOn
            });
        }

        [HttpGet("[action]/{key}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos de Acceso al Logo del Eveto", typeof(Cloud.Api.Common.Models.EventBlobSharedAccessSignatureModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "No autorizado a acceder al Album!")]
        public IActionResult Event(System.Guid key)
        {
            var entity = eventRepository.Get(key);

            // TODO Assert album accesibility!

            Providers.ImagesBlobContainerClientProvider.Instance.TargetDefaultBlobContainer(configuration);

            var expiresOn = System.DateTimeOffset.UtcNow.AddHours(1);

            var blobClient = Providers.ImagesBlobContainerClientProvider.Instance.GetBlobContainerClient().GetBlobClient(entity.Logo.Id.ToBlobId(entity.Logo.ContentType));

            return Ok(new Cloud.Api.Common.Models.EventBlobSharedAccessSignatureModel
            {
                LogoImageBlobUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.List | Azure.Storage.Sas.BlobSasPermissions.Read, expiresOn)
            });
        }
    }
}