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
        private readonly IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository;

        public AlbumsController(IEntityRepository<Persistence.Entities.Terminal, string> terminalRepository, IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository, Microsoft.Extensions.Localization.IStringLocalizer<AlbumsController> localizer, ILogger<AlbumsController> logger) : base(localizer, logger)
        {
            this.albumRepository = albumRepository;
            this.terminalRepository = terminalRepository;
        }

        [HttpGet("{key}/stats")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Estadísticas del Album", typeof(Cloud.Api.Common.Models.AlbumStatsModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "Album no vinculado al usuario!")]
        public virtual IActionResult GetStatsById(System.Guid key)
        {
            try
            {
                var terminal = terminalRepository.Get(User.Identity.Name);

                if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == key))
                {
                    return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
                }

                var album = albumRepository.Get(key);

                if (!album.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                var model = new Cloud.Api.Common.Models.AlbumStatsModel
                {
                    PostCount = album.Contents.Count,
                    FallowerCount = terminalRepository.Count(entity => entity.AlbumBridges.Any(bridge => bridge.AlbumId == key))
                };

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

        [HttpGet("id/{id}")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Locked, "Album protegido!")]
        public virtual IActionResult GetById(System.Guid id)
        {
            try
            {
                var terminal = terminalRepository.Get(User.Identity.Name);

                var album = albumRepository.Get(id);

                if (!album.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                if (album.LockBridges.Count > 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.Locked);
                }

                if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == album.Id))
                {
                    terminal.AlbumBridges.Add(new Persistence.Entities.TerminalAlbumBridge
                    {
                        TerminalId = terminal.Id,
                        AlbumId = album.Id
                    });

                    terminalRepository.Update(terminal, "system");
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(album);

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
                var terminal = terminalRepository.Get(User.Identity.Name);

                var album = albumRepository.FindOneBy(e => e.Alias == alias);

                if (album == null) return NoContent();

                if (!album.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                if (album.LockBridges.Count > 0)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.Locked);
                }

                if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == album.Id))
                {
                    terminal.AlbumBridges.Add(new Persistence.Entities.TerminalAlbumBridge
                    {
                        TerminalId = terminal.Id,
                        AlbumId = album.Id
                    });

                    terminalRepository.Update(terminal, "system");
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(album);

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
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "Credencial no válida!")]
        public virtual IActionResult GetByIdAndPassword(System.Guid id, string key)
        {
            try
            {
                var terminal = terminalRepository.Get(User.Identity.Name);

                var album = albumRepository.Get(id);

                if (!album.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                var password = album.LockBridges.First(b => b.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).SerializedContext;

                if (password != key)
                {
                    return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
                }

                if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == album.Id))
                {
                    terminal.AlbumBridges.Add(new Persistence.Entities.TerminalAlbumBridge
                    {
                        TerminalId = terminal.Id,
                        AlbumId = album.Id
                    });

                    terminalRepository.Update(terminal, "system");
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(album);

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
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Album", typeof(Cloud.Api.Common.Models.AlbumModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.PreconditionFailed, "Album no habilitado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Album no encontrado!")]
        [SwaggerResponse((int)System.Net.HttpStatusCode.Forbidden, "Credencial no válida!")]
        public virtual IActionResult GetByAliasAndPassword(string alias, string key)
        {
            try
            {
                var terminal = terminalRepository.Get(User.Identity.Name); 
                
                var album = albumRepository.FindOneBy(e => e.Alias == alias);

                if (album == null) return NoContent();

                if (!album.Enabled)
                {
                    return StatusCode((int)System.Net.HttpStatusCode.PreconditionFailed);
                }

                var password = album.LockBridges.First(b => b.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).SerializedContext;

                if (password != key)
                {
                    return Forbid(new string[] { Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme });
                }

                if (!terminal.AlbumBridges.Any(bridge => bridge.AlbumId == album.Id))
                {
                    terminal.AlbumBridges.Add(new Persistence.Entities.TerminalAlbumBridge
                    {
                        TerminalId = terminal.Id,
                        AlbumId = album.Id
                    });

                    terminalRepository.Update(terminal, "system");
                }

                var model = new Cloud.Api.Common.Models.AlbumModel();

                model.Import(album);

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
