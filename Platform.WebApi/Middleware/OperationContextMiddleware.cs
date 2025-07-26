using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Platform.WebApi.Context;

namespace Platform.WebApi.Middleware;

public class OperationContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Endpoint? endpoint = context.GetEndpoint();

        var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (actionDescriptor == null)
        {
            await next.Invoke(context);
            
            return;
        }
        
        var operationContextProvider = context.RequestServices.GetRequiredService<IOperationContextProvider>();
        var operationContext = new OperationContext
        {
            TraceId = context.TraceIdentifier,
            User = (ICurrentUser?)context.Items["User"],
            OperationInfo = new OperationInfo
            {
                OperationName = $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}",
                OperationStartDateTimeUtc = DateTime.UtcNow   
            }
        };

        operationContextProvider.SetOperationContext(operationContext);
        
        await next.Invoke(context);
    }
}