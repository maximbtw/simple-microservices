using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Utilities;

namespace Platform.Client.Http;

public abstract class HttpClientBase
{
    private const string BaseHttpClientPrefix = "ServiceHttpClient";

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly IHttpClientObserver? _observer;
    private readonly string _serviceName;

    protected HttpClientBase(HttpClient httpClient, string? serviceName = null, IHttpClientObserver? observer = null)
    {
        _httpClient = httpClient;
        _observer = observer;
        _serviceName = serviceName ?? GetDefaultServiceName();
    }

    protected async Task<TResponse> PostAsync<TRequest, TResponse>(
        TRequest request,
        HttpMediaType mediaType = HttpMediaType.Json,
        CancellationToken ct = default,
        [CallerMemberName] string operationName = "")
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _observer?.OnRequestStart(operationName);

            HttpRequestMessage httpRequest = CreatePostRequest(request, mediaType, operationName);

            HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, ct);

            var response = await HandleResponse<TResponse>(httpResponse, mediaType, ct);

            _observer?.OnRequestSuccess(operationName, httpResponse.StatusCode);

            return response;
        }
        catch (HttpRequestException ex)
        {
            _observer?.OnRequestError(operationName, ex);
            throw;
        }
        catch (Exception ex)
        {
            _observer?.OnUnexpectedError(operationName, ex);
            throw;
        }
        finally
        {
            sw.Stop();

            _observer?.OnRequestEnd(operationName, sw.Elapsed);
        }
    }

    private HttpRequestMessage CreatePostRequest<TRequest>(
        TRequest request,
        HttpMediaType mediaType,
        string operationName)
    {
        var uri = new Uri($"{_serviceName}/{operationName}", UriKind.Relative);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);

        if (mediaType == HttpMediaType.Json)
        {
            string stringRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);
            httpRequest.Content = new StringContent(stringRequest, Encoding.UTF8, mediaType.GetContentType());
        }
        else if (mediaType == HttpMediaType.Protobuf)
        {
            httpRequest.Content = new ByteArrayContent(SerializationHelper.SerializeAsProto(request));
        }
        else
        {
            throw new NotSupportedException($"Unknown media type: {mediaType}");
        }
        
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType.GetContentType()));
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType.GetContentType());

        return httpRequest;
    }

    private async Task<TResponse> HandleResponse<TResponse>(
        HttpResponseMessage httpResponse, 
        HttpMediaType mediaType,
        CancellationToken ct)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            if (mediaType == HttpMediaType.Json)
            {
                await using Stream stream = await httpResponse.Content.ReadAsStreamAsync(ct);

                return (await JsonSerializer.DeserializeAsync<TResponse>(stream, _jsonSerializerOptions, ct))!;
            }

            if (mediaType == HttpMediaType.Protobuf)
            {
                await using Stream stream = await httpResponse.Content.ReadAsStreamAsync(ct);

                return SerializationHelper.DeserializeFromProto<TResponse>(stream);
            }

            throw new NotSupportedException($"Unknown media type: {mediaType}");

        }

        string responseDetail = await httpResponse.Content.ReadAsStringAsync(ct);

        throw new HttpRequestException(responseDetail);
    }

    private string GetDefaultServiceName()
    {
        var typeName = GetType().Name;
        if (!typeName.Contains(BaseHttpClientPrefix))
        {
            throw new ArgumentException($"The service client name must contain the prefix '{BaseHttpClientPrefix}'");
        }
            
        return typeName.Replace(BaseHttpClientPrefix, "");
    }
}