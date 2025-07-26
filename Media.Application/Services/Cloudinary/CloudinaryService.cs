using Media.Contracts.Cloudinary;
using Media.Contracts.Cloudinary.DeleteImageFile;
using Media.Contracts.Cloudinary.UpdateImageFile;
using Media.Contracts.Cloudinary.UploadImageFile;

namespace Media.Application.Services.Cloudinary;

internal class CloudinaryService : ICloudinaryService
{
    private readonly CloudinaryServiceUploadImageFileHandler _uploadImageFileHandler;
    private readonly CloudinaryServiceUpdateImageFileHandler _updateImageFileHandler;
    private readonly CloudinaryServiceDeleteImageFileHandler _deleteImageFileHandler;

    public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
    {
        _uploadImageFileHandler = new CloudinaryServiceUploadImageFileHandler(cloudinary);
        _updateImageFileHandler = new CloudinaryServiceUpdateImageFileHandler(this);
        _deleteImageFileHandler = new CloudinaryServiceDeleteImageFileHandler(cloudinary);
    }

    public async Task<UploadImageFileResponse> UploadImageFile(UploadImageFileRequest request) =>
        await _uploadImageFileHandler.UpdateImageFile(request);

    public async Task<UpdateImageFileResponse> UpdateImageFile(UpdateImageFileRequest request) =>
        await _updateImageFileHandler.UpdateImageFile(request);

    public async Task<DeleteImageFileResponse> DeleteImageFile(DeleteImageFileRequest request) =>
        await _deleteImageFileHandler.DeleteImageFile(request);
}