using System.Linq;

namespace EM.Cloud.Filters.Swagger
{
    public class OperationSwaggerFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        private readonly Microsoft.OpenApi.Models.OpenApiSecurityScheme openApiSecurityScheme;

        public OperationSwaggerFilter(Microsoft.OpenApi.Models.OpenApiSecurityScheme openApiSecurityScheme)
        {
            this.openApiSecurityScheme = openApiSecurityScheme;
        }

        public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any() || context.MethodInfo.GetCustomAttributes(true).OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Acceso no autorizado!" });

                operation.Security = new System.Collections.Generic.List<Microsoft.OpenApi.Models.OpenApiSecurityRequirement>
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        { openApiSecurityScheme, new System.Collections.Generic.List<string> {} }
                    }
                };
            }
        }
    }
}
