using CloudinaryDotNet;
using Media.Application.Infrastructure.Configuration;
using Media.Application.Persistence.Image;
using Media.Application.Services.Cloudinary;
using Media.Application.Services.Image;
using Media.Contracts.Cloudinary;
using Media.Contracts.Image;
using Media.Contracts.Persistence.Image;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Media.Application;

public static class DependencyInjectionExtensions
{
    public static void BindOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection("Configuration:DatabaseOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<CloudinaryOptions>()
            .Bind(configuration.GetSection("Configuration:CloudinaryOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
    
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IImageRepository, ImageRepository>();
        
        services.AddTransient<ICloudinaryService, CloudinaryService>();
        services.AddTransient<IImageService, ImageService>();
    }
    
    public static void AddCloudinary(this IServiceCollection services, CloudinaryOptions options)
    {
        services.AddSingleton(_ =>
        {
            var account = new Account(options.CloudName, options.ApiKey, options.ApiSecret);
            return new Cloudinary(account);
        });
    }
}