namespace EM.Cloud.Filters.Swagger.Operation
{
    public class PublicOperationSwaggerFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
        {
            operation.Deprecated = true;
        }
    }
}
