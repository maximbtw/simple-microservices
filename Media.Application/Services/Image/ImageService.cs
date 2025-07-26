using Media.Application.Persistence.Image;
using Media.Contracts.Cloudinary;
using Media.Contracts.Image;
using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;
using Media.Contracts.Persistence.Image;
using Platform.Domain.EF;

namespace Media.Application.Services.Image;

public class ImageService : IImageService
{
    private readonly ImageServiceUploadImageHandler _uploadImageHandler;
    private readonly ImageServiceUpdateImageHandler _updateImageHandler;
    private readonly ImageServiceDeleteImageHandler _deleteImageHandler;

    public ImageService(
        IImageRepository imageRepository,
        ICloudinaryService cloudinaryService,
        IDbScopeProvider scopeProvider)
    {
        _uploadImageHandler = new ImageServiceUploadImageHandler(
            imageRepository,
            cloudinaryService,
            scopeProvider);

        _updateImageHandler = new ImageServiceUpdateImageHandler(
            imageRepository,
            cloudinaryService,
            scopeProvider);
        
        _deleteImageHandler = new ImageServiceDeleteImageHandler(
            imageRepository,
            cloudinaryService,
            scopeProvider);
    }

    public async Task<UploadImageResponse> UploadImage(UploadImageRequest request) =>
        await _uploadImageHandler.UploadImage(request);

    public async Task<UpdateImageResponse> UpdateImage(UpdateImageRequest request) =>
        await _updateImageHandler.UpdateImage(request);

    public async Task<DeleteImageResponse> DeleteImage(DeleteImageRequest request) =>
        await _deleteImageHandler.DeleteImage(request);
}