using EM.Cloud.Api.Common.Models;
using EM.Cloud.Api.Common.Models.Filter;
using EM.Cloud.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Api.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace EM.Cloud.Controllers.Api
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : AbstractEntityApiManagementFilteredController<Content, System.Guid, ContentInputModel, ContentDisplayModel, ContentFilterModel>
    {
        public ContentsController(IEntityRepository<Content, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<ContentsController> localizer, ILogger<ContentsController> logger) : base(repository, localizer, logger)
        {
        }
        
        [System.Obsolete]

        public override IActionResult Get()
        {
            //var all = Repository.GetAll().ToArray();

            //var result = all.Select(entity => EntityToDisplayModel(entity)).ToArray();

            //return Ok(result);

            return NotFound();
        }

        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Datos del Contenido", typeof(ContentDisplayModel))]
        [SwaggerResponse((int)System.Net.HttpStatusCode.NoContent, "Contenido no encontrado!")]
        public override IActionResult Get(System.Guid key)
        {
            return base.Get(key);
        }

        [System.Obsolete]
        public override IActionResult GetPaginate(int sheet = 1, int limit = 25)
        {
            //return base.GetPaginate(sheet, limit);

            return NotFound();
        }

        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Lista de Contenidos", typeof(ContentDisplayModel[]))]
        public override IActionResult GetFiltered([FromBody] ContentFilterModel filterModel)
        {
            return base.GetFiltered(filterModel);
        }

        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, "Paginación de Contenidos", typeof(SmartSolucionesCuba.SAPRESSC.Core.Common.Models.PaginationModel<ContentDisplayModel>))]
        public override IActionResult GetPaginateFiltered([FromBody] ContentFilterModel filterModel, int sheet = 1, int limit = 25)
        {
            return base.GetPaginateFiltered(filterModel, sheet, limit);
        }

        [System.Obsolete]
        public override IActionResult Create([FromBody] ContentInputModel inputModel)
        {
            return NotFound();
        }

        [System.Obsolete]
        public override IActionResult Edit([FromBody] ContentInputModel inputModel)
        {
            return NotFound();
        }

        [System.Obsolete]
        public override IActionResult Delete(System.Guid key)
        {
            return NotFound();
        }

        protected override ContentDisplayModel EntityToDisplayModel(Content entity)
        {
            if (entity is PhotoSessionContent)
            {
                var result = new PhotoSessionContentDisplayModel();

                result.Import(entity as PhotoSessionContent);

                return result;
            }

            if (entity is PictureContent)
            {
                var result = new PictureContentDisplayModel();

                result.Import(entity as PictureContent);

                return result;
            }

            // TODO Return concrete model for the rest of Content Types!

            throw new System.InvalidOperationException();
        }
    }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
}
