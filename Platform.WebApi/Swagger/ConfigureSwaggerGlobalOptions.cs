using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Platform.WebApi.Swagger;

public class ConfigureSwaggerGlobalOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.SchemaFilter<NullableRemoverFilter>();
        
        options.AddSecurityDefinition("X-User", new OpenApiSecurityScheme
        {
            Description = "Base64-encoded user info (id, login, role). Example: eyJpZCI6MX0=",
            Name = "X-User",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });
        
        options.OperationFilter<RequireRoleOperationFilter>();
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}