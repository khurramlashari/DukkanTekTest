using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ProductsInventory.Core
{
    /// <summary>
    /// Filter that Implements IOperationFilter
    /// </summary>
    public class RemoveVersionFromParameter : IOperationFilter
    {
        /// <summary>
        /// Apply filter
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }
}
