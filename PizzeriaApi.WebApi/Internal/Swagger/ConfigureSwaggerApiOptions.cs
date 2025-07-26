using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Platform.WebApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PizzeriaApi.WebApi.Internal.Swagger;

internal class ConfigureSwaggerApiOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.SchemaFilter<NullableRemoverFilter>();
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        
        options.OperationFilter<RequireRoleOperationApiFilter>();
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}