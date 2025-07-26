using Media.Contracts.Cloudinary.DeleteImageFile;
using Media.Contracts.Cloudinary.UpdateImageFile;
using Media.Contracts.Cloudinary.UploadImageFile;

namespace Media.Contracts.Cloudinary;

public interface ICloudinaryService
{
    Task<UploadImageFileResponse> UploadImageFile(UploadImageFileRequest request);

    Task<UpdateImageFileResponse> UpdateImageFile(UpdateImageFileRequest request);

    Task<DeleteImageFileResponse> DeleteImageFile(DeleteImageFileRequest request);
}