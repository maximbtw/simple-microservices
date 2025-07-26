using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;

namespace Media.Contracts.Image;

public interface IImageService
{
    Task<UploadImageResponse> UploadImage(UploadImageRequest request);
    
    Task<UpdateImageResponse> UpdateImage(UpdateImageRequest request);
    
    Task<DeleteImageResponse> DeleteImage(DeleteImageRequest request);
}