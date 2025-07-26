using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Application.Infrastructure.Loggers;
using PizzeriaApi.Contracts;

namespace PizzeriaApi.WebApi.Internal;

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
            
            var response = new ApiResponseBase();
            var error = Error.CreateError(ErrorCode.ServerError);
        
            response.AddError(error);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(response, jsonOptions.JsonSerializerOptions);
        }
    }
}