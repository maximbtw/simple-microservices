using Auth.WebApi.Internal.Loggers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Platform.WebApi;

namespace Auth.WebApi.Internal;

internal static class ExceptionHandler
{
    public static async Task Handle(HttpContext context, JsonOptions jsonOptions)
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        Exception? exception = exceptionHandlerFeature?.Error;

        if (exception != null)
        {
            context.Items["Exception"] = exception.ToString();
            
            OperationErrorsLogger.Log(context.TraceIdentifier, exception);

            context.Response.StatusCode = 200;
            await ErrorResponseWriter.WriteInternalServerError(context, jsonOptions.JsonSerializerOptions);
        }
    }
}