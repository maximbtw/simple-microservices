using Media.Contracts.Image;
using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;
using Platform.Client.Http;

namespace Media.Client;

public class ImageServiceHttpClient(HttpClient httpClient, IHttpClientObserver? observer = null)
    : HttpClientBase(httpClient, null, observer), IImageService
{
    public async Task<UploadImageResponse> UploadImage(UploadImageRequest request) =>
        await PostAsync<UploadImageRequest, UploadImageResponse>(request);

    public async Task<UpdateImageResponse> UpdateImage(UpdateImageRequest request) =>
        await PostAsync<UpdateImageRequest, UpdateImageResponse>(request);

    public async Task<DeleteImageResponse> DeleteImage(DeleteImageRequest request) =>
        await PostAsync<DeleteImageRequest, DeleteImageResponse>(request);
}