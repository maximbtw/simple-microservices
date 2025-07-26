namespace Platform.Client.Http;

internal static class HttpMediaTypeExtension
{
    public static string GetContentType(this HttpMediaType mediaType)
    {
        return mediaType switch
        {
            HttpMediaType.Json => "application/json",
            HttpMediaType.Protobuf => "application/x-protobuf",
            _ => throw new ArgumentOutOfRangeException(nameof(mediaType), mediaType, null)
        };
    }
}