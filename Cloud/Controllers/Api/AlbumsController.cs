using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using System.Linq;

namespace EM.Cloud.Controllers.Api
{
    [Route("api/[controller]")]
    public class AlbumsController : SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Controllers.BaseWebController
    {
        private readonly IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository;

        public AlbumsController(IEntityRepository<Persistence.Entities.Album, System.Guid> albumRepository, Microsoft.Extensions.Localization.IStringLocalizer<AlbumsController> localizer, ILogger<AlbumsController> logger) : base(localizer, logger)
        {
            this.albumRepository = albumRepository;
        }

        [HttpGet("id/{id}")]
        public virtual IActionResult GetById(System.Guid id)
        {
            try
            {
                var entity = albumRepository.Get(id);

                if (!entity.Enabled)
                {
                    return NoContent();
                }

                if (entity.LockBridges.Count > 0)
                {
                    return Unauthorized();
                }

                var model = new Cloud.Api.Common.Models.AlbumDisplayModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                return NoContent();
            }
        }

        [HttpGet("alias/{alias}")]
        public virtual IActionResult GetByAlias(string alias)
        {
            try
            {
                var entity = albumRepository.FindOneBy(e => e.Alias == alias);

                if (!entity.Enabled)
                {
                    return NoContent();
                }

                if (entity.LockBridges.Count > 0)
                {
                    return Unauthorized();
                }

                var model = new Cloud.Api.Common.Models.AlbumDisplayModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                return NoContent();
            }
        }

        [HttpGet("id/{id}/password/{key}")]
        public virtual IActionResult GetByIdAndPassword(System.Guid id, string key)
        {
            try
            {
                var entity = albumRepository.Get(id);

                if (!entity.Enabled)
                {
                    return NoContent();
                }

                var password = entity.LockBridges.First(b => b.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).SerializedContext;

                if (password != key)
                {
                    return Unauthorized();
                }

                var model = new Cloud.Api.Common.Models.AlbumDisplayModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                return NoContent();
            }
        }

        [HttpGet("alias/{alias}/password/{key}")]
        public virtual IActionResult GetByAliasAndPassword(string alias, string key)
        {
            try
            {
                var entity = albumRepository.FindOneBy(e => e.Alias == alias);

                if (!entity.Enabled)
                {
                    return NoContent();
                }

                var password = entity.LockBridges.First(b => b.LockId == Extend.Lock.Pioneer.Descriptors.PasswordAlbumLockDescriptor.ID).SerializedContext;

                if (password != key)
                {
                    return Unauthorized();
                }

                var model = new Cloud.Api.Common.Models.AlbumDisplayModel();

                model.Import(entity);

                return Ok(model);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                return NoContent();
            }
        }
    }
}
