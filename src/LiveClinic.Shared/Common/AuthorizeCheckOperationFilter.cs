using System.Collections.Generic;
using System.Linq;
using LiveClinic.Shared.Common.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LiveClinic.Shared.Common
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly LiveAuthSetting _liveAuthSetting;

        public AuthorizeCheckOperationFilter(LiveAuthSetting liveAuthSetting)
        {
            _liveAuthSetting = liveAuthSetting;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType != null && (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                                                            || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme {Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"}
                            }
                        ] = new[] { _liveAuthSetting.Scope }
                    }
                };

            }
        }
    }
}