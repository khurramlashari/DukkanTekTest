using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ProductsInventory.Core
{
    // Swagger IOperationFilter implementation that will decide which api action needs authorization
    internal class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AllowAnonymousAttribute>()
                .Any();

            if (!hasAuthorize)
            {

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {

                        [oAuthScheme] = new [] { IdentityServerConfig.ApiName}
                    }
                };
            }
        }
    }
}
