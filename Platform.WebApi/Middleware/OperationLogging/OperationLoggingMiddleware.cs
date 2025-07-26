using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IO;
using Platform.WebApi.Context;

namespace Platform.WebApi.Middleware.OperationLogging;

public class OperationLoggingMiddleware
{
    private const int MaxCountResponseLength = 10000;
    
    private readonly RequestDelegate _next;
    private readonly JsonOptions _jsonOptions;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

    public OperationLoggingMiddleware(RequestDelegate next, IOptions<JsonOptions> jsonOptions)
    {
        _next = next;
        _jsonOptions = jsonOptions.Value;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Endpoint? endpoint = context.GetEndpoint();

        var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (actionDescriptor == null)
        {
            await _next.Invoke(context);

            return;
        }

        using MemoryStream response = _recyclableMemoryStreamManager.GetStream();

        long memoryBeforeRequest = GC.GetTotalMemory(forceFullCollection: false);
        int generation0CollectionCountOnStart = GC.CollectionCount(generation: 0);
        int generation1CollectionCountOnStart = GC.CollectionCount(generation: 1);
        int generation2CollectionCountOnStart = GC.CollectionCount(generation: 2);

        string jsonRequest = string.Empty;
        string jsonResponse = string.Empty;
        string exception = string.Empty;
        try
        {
            jsonRequest = await GetJsonRequestBody(context.Request);
            if (string.IsNullOrEmpty(jsonRequest))
            {
                await ErrorResponseWriter.WriteInvalidRequest(
                    context, 
                    description: "Request is empty.",
                    _jsonOptions.JsonSerializerOptions);
                
                return;
            }

            Stream originalResponseStream = context.Response.Body;
            await using RecyclableMemoryStream responseStream = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseStream;

            await _next.Invoke(context);

            jsonResponse = await GetJsonResponseBody(
                context.Response,
                responseStream,
                originalResponseStream,
                response);
        }
        catch (Exception ex)
        {
            exception  = ex.Message;
            
            throw;
        }
        finally
        {
            var operationContextProvider = context.RequestServices.GetRequiredService<IOperationContextProvider>();

            OperationContext operationContext = operationContextProvider.GetContext()!;

            OperationRequestsLogger.Log(
                operationContext,
                jsonRequest,
                jsonResponse,
                exception,
                context.Connection.RemoteIpAddress?.ToString(),
                memoryBeforeRequest,
                generation0CollectionCountOnStart,
                generation1CollectionCountOnStart,
                generation2CollectionCountOnStart);
        }
    }

    private static async Task<string> GetJsonRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        
        request.Body.Seek(0, SeekOrigin.Begin);
        string bodyAsText = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        
        if (string.IsNullOrEmpty(bodyAsText))
        {
            return string.Empty;
        }

        bodyAsText = Regex.Replace(bodyAsText, @"\r\n?|\n| ", string.Empty);

        return bodyAsText;
    }

    private static async Task<string> GetJsonResponseBody(
        HttpResponse httpResponse, 
        RecyclableMemoryStream responseStream,
        Stream originalResponseStream,
        MemoryStream response)
    {
        httpResponse.Body.Seek(offset: 0, SeekOrigin.Begin);
        await responseStream.CopyToAsync(response);
        response.Seek(offset: 0, SeekOrigin.Begin);
        httpResponse.Body.Seek(offset: 0, SeekOrigin.Begin);
        
        await responseStream.CopyToAsync(originalResponseStream);

        if (response is { Length: < MaxCountResponseLength })
        {
            response.Seek(offset: 0, SeekOrigin.Begin);
            var streamReader = new StreamReader(response);
            char[] buffer = new char[MaxCountResponseLength];
            int charsRead = await streamReader.ReadAsync(buffer, index: 0, MaxCountResponseLength);

            return new string(buffer, startIndex: 0, charsRead);
        }

        return string.Empty;
    }
}