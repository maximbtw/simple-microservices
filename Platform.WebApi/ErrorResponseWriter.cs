using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Platform.Core.Operations;

namespace Platform.WebApi;

public static class ErrorResponseWriter
{
    public static async Task WriteAccessDenied(
        HttpContext context,
        AccessDeniedReason reason,
        JsonSerializerOptions jsonSerializerOptions) =>
        await context.Response.WriteAsJsonAsync(new OperationResponseBase
        {
            Ok = false,
            Errors = new OperationResponseStandardErrors
            {
                AccessDenied = true,
                AccessDeniedReason = reason
            }
        }, jsonSerializerOptions);

    public static async Task WriteInternalServerError(
        HttpContext context, 
        JsonSerializerOptions jsonSerializerOptions) =>
        await context.Response.WriteAsJsonAsync(new OperationResponseBase
        {
            Ok = false,
            Errors = new OperationResponseStandardErrors
            {
                InternalServerError = true
            }
        }, jsonSerializerOptions);

    public static async Task WriteInvalidRequest(
        HttpContext context, 
        string description,
        JsonSerializerOptions jsonSerializerOptions) =>
        await context.Response.WriteAsJsonAsync(new OperationResponseBase
        {
            Ok = false,
            Errors = new OperationResponseStandardErrors
            {
                InvalidRequest = true,
                InvalidRequestDescription = description
            }
        }, jsonSerializerOptions);
}