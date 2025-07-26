using Microsoft.OpenApi.Models;
using Platform.WebApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PizzeriaApi.WebApi.Internal.Swagger;

internal class RequireRoleOperationApiFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        object[] methodAttributes = context.MethodInfo.GetCustomAttributes(true);
        object[] classAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? [];

        List<RequireRolesAttribute> requireRoleAttributes = methodAttributes
            .Concat(classAttributes)
            .OfType<RequireRolesAttribute>()
            .ToList();

        if (!requireRoleAttributes.Any())
        {
            return;
        }
        
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            }
        };
        
        
        IEnumerable<string> roles = requireRoleAttributes.SelectMany(x => x.Roles).Distinct().Select(x=>x.ToString());
        
        string rolesList = string.Join("", roles.Select(role => $"<li>{role}</li>"));
        var rolesHtml = $"<br/><b>Требуемые роли:</b><ul>{rolesList}</ul>";
        operation.Description = (operation.Description ?? string.Empty) + rolesHtml;
    }
}