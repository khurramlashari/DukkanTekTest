using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace ProductsInventory.Core
{
    /// <summary>
    /// Filter ReplaceVersionWithExactValueInPath
    /// </summary>
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        /// <summary>
        /// Apply filter
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = paths;

        }


    }
}
