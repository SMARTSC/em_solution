using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace EM.Cloud.Controllers.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository;

        public AlbumsController(IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository, Microsoft.Extensions.Localization.IStringLocalizer<AlbumsController> localizer, ILogger<AlbumsController> logger) : base(localizer, logger)
        {
            this.albumRepository = albumRepository;
        }

        [HttpGet("id/{id}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Locked, "Album protegido!")]
        public virtual IActionResult GetById(System.Guid id)
        {
            try
            {
                var entity = albumRepository.Get(id);

                if (!entity.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                if (entity.LockBridges.Count > 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.Locked);
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.InvalidOperationException)
            {
                return NoContent();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                throw e;
            }
        }

        [HttpGet("alias/{alias}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Locked, "Album protegido!")]
        public virtual IActionResult GetByAlias(string alias)
        {
            try
            {
                var entity = albumRepository.FindOneBy(e => e.Alias == alias);

                if (!entity.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                if (entity.LockBridges.Count > 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.Locked);
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.InvalidOperationException)
            {
                return NoContent();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                throw e;
            }
        }

        [HttpGet("id/{id}/password/{key}")]
        [SwaggerResponse(200, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Unauthorized, "Acceso al Album no autorizado!")]
        public virtual IActionResult GetByIdAndPassword(System.Guid id, string key)
        {
            try
            {
                var entity = albumRepository.Get(id);

                if (!entity.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                var password = entity.LockBridges.First(b => b.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).SerializedContext;

                if (password != key)
                {
                    return Unauthorized();
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.InvalidOperationException)
            {
                return NoContent();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                throw e;
            }
        }

        [HttpGet("alias/{alias}/password/{key}")]
        [SwaggerResponse(200, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Unauthorized, "Acceso al Album no autorizado!")]
        public virtual IActionResult GetByAliasAndPassword(string alias, string key)
        {
            try
            {
                var entity = albumRepository.FindOneBy(e => e.Alias == alias);

                if (!entity.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                var password = entity.LockBridges.First(b => b.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).SerializedContext;

                if (password != key)
                {
                    return Unauthorized();
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.InvalidOperationException)
            {
                return NoContent();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                throw e;
            }
        }
    }
}
