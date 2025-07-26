using Microsoft.AspNetCore.Http;
using Platform.Client.Http;
using Platform.WebApi.Context;
using Utilities;

namespace Platform.WebApi;

public class CustomHeaderHttpMessageHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (contextAccessor.HttpContext == null)
        {
            return base.SendAsync(request, cancellationToken);
        }

        HttpContext context = contextAccessor.HttpContext!;

        var traceId = context.Items["TraceId"]?.ToString();
        if (!string.IsNullOrEmpty(traceId) && !request.Headers.Contains(HeaderConstants.HeaderTraceId))
        {
            request.Headers.Add(HeaderConstants.HeaderTraceId, traceId);
        }

        var user = (ICurrentUser?)context.Items["User"];
        if (user != null && !request.Headers.Contains(HeaderConstants.HeaderUser))
        {
            // TODO: Правильней иметь ключ по которму бы шифровался внутрений ключ.
            // В целом можно иметь общий JWT ключ, но такой подход мне нравиться больше
            string userBase64 = SerializationHelper.SerializeToBase64(user);

            request.Headers.Add(HeaderConstants.HeaderUser, userBase64);
        }

        return base.SendAsync(request, cancellationToken);
    }
}