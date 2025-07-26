using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Platform.WebApi.Swagger;

internal class RequireRoleOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        object[] methodAttributes = context.MethodInfo.GetCustomAttributes(true);
        object[] classAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? Array.Empty<object>();
        
        List<RequireRolesAttribute> requireRoleAttributes = methodAttributes
            .Concat(classAttributes)
            .OfType<RequireRolesAttribute>()
            .ToList();

        if (!requireRoleAttributes.Any())
        {
            return;
        }
        
        operation.Security ??= new List<OpenApiSecurityRequirement>();

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "X-User" 
                    },
                    In = ParameterLocation.Header,
                    Name = "X-User",
                    Type = SecuritySchemeType.ApiKey
                },
                new List<string>()
            }
        });
        
        string[] roles = requireRoleAttributes
            .SelectMany(attr => attr.Roles)
            .Distinct()
            .Select(r => r.ToString()).ToArray();

        if (roles.Any())
        {
            string rolesList = string.Join("", roles.Select(role => $"<li>{role}</li>"));
            string rolesHtml = $"<br/><b>🔐 Требуемые роли:</b><ul>{rolesList}</ul>";
            operation.Description = (operation.Description ?? string.Empty) + rolesHtml;
        }
    }
}