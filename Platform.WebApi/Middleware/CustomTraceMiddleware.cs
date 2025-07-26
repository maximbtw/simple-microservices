using Microsoft.AspNetCore.Http;
using Platform.Client.Http;

namespace Platform.WebApi.Middleware;

public class CustomTraceMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string headerTraceId = context.Request.Headers[HeaderConstants.HeaderTraceId].ToString();
        string traceId = string.IsNullOrEmpty(headerTraceId) ? Guid.NewGuid().ToString() : headerTraceId;

        context.TraceIdentifier = traceId;
        context.Items["TraceId"] = traceId;

        await next.Invoke(context);
    }
}