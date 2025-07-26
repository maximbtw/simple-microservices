using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Platform.Client.Http;
using Platform.Core.Operations;
using Platform.Domain;
using Platform.WebApi.Context;
using Utilities;

namespace Platform.WebApi.Middleware;

public class CustomAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JsonOptions _jsonOptions;

    public CustomAuthorizationMiddleware(RequestDelegate next, IOptions<JsonOptions> jsonOptions)
    {
        _next = next;
        _jsonOptions = jsonOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Endpoint? endpoint = context.GetEndpoint();
        
        var roleAttribute = endpoint?.Metadata.GetMetadata<RequireRolesAttribute>();
        if (roleAttribute == null)
        {
            await _next(context);

            return;
        }
        
        string userHeader = context.Request.Headers[HeaderConstants.HeaderUser].ToString();
        if (string.IsNullOrEmpty(userHeader))
        {
            await ErrorResponseWriter.WriteAccessDenied(
                context,
                AccessDeniedReason.AuthorizationHeaderNotFound,
                _jsonOptions.JsonSerializerOptions);

            return;
        }
        
        var user = SerializationHelper.DeserializeFromBase64<CurrentUser>(userHeader);
        
        bool operationAllowedForUser = roleAttribute.Roles.Any(x => user.Role == x);
        if (!operationAllowedForUser)
        {
            await ErrorResponseWriter.WriteAccessDenied(
                context,
                AccessDeniedReason.OperationAccessDenied,
                _jsonOptions.JsonSerializerOptions);
        }
                
        context.Items["User"] = user;
        
        await _next(context);
    }
    
    private class CurrentUser : ICurrentUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}