using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PizzeriaApi.Contracts;
using PizzeriaApi.Contracts.Providers;
using PizzeriaApi.Contracts.Shared;
using Platform.Domain;
using Platform.WebApi.Context;

namespace PizzeriaApi.WebApi.Middleware;

internal class ApiAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAccountUserProvider _accountUserProvider;
    private readonly JsonOptions _jsonOptions;

    public ApiAuthorizationMiddleware(
        RequestDelegate next,
        IAccountUserProvider accountUserProvider,
        IOptions<JsonOptions> jsonOptions)
    {
        _next = next;
        _accountUserProvider = accountUserProvider;
        _jsonOptions = jsonOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated is false or null)
        {
            await _next(context);

            return;
        }

        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        if (!user.IsActive)
        {
            var response = new ApiResponseBase();
            var error = Error.CreateError(ErrorCode.NotAuthorized, "User is not active.");
        
            response.AddError(error);
        
            await context.Response.WriteAsJsonAsync(response, _jsonOptions.JsonSerializerOptions);
            
            return;
        }

        context.Items["User"] = new CurrentUser
        {
            Id = user.Id,
            Role = user.Role,
            Login = user.Login
        };
            
        await _next(context);
    }
    
    private class CurrentUser : ICurrentUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}