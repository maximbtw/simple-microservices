using Media.Application.Persistence.Image;
using Media.Contracts.Cloudinary;
using Media.Contracts.Cloudinary.DeleteImageFile;
using Media.Contracts.Image.Delete;
using Media.Contracts.Persistence.Image;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Media.Application.Services.Image;

public class ImageServiceDeleteImageHandler
{
    private readonly IImageRepository _imageRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IDbScopeProvider _scopeProvider;

    public ImageServiceDeleteImageHandler(
        IImageRepository imageRepository, 
        ICloudinaryService cloudinaryService,
        IDbScopeProvider scopeProvider)
    {
        _imageRepository = imageRepository;
        _cloudinaryService = cloudinaryService;
        _scopeProvider = scopeProvider;
    }
    
    public async Task<DeleteImageResponse> DeleteImage(DeleteImageRequest request)
    {
        var response = new DeleteImageResponse();

        ImageDto? image;
        await using (OperationReaderScope scope = _scopeProvider.GetReaderScope())
        {
            image = await _imageRepository.GetOrNull(request.ImageId, scope);
            if (image == null)
            {
                return response;
            }
        }

        DeleteImageFileResponse deleteFileResponse = await Delete(image.PublicId);
        if (!deleteFileResponse.Ok)
        {
            return response;
        }
        
        await using (OperationModificationScope scope = _scopeProvider.GetModificationScope())
        {
            var parameters = new EntityDeleteParameters { EntityId = image.Id };
            EntityDeleteResult result = await _imageRepository.Delete(parameters, scope);
            if (!result.Ok)
            {
                return response;
            }
        }
        
        response.Ok = true;
        return response;
    }
    
    private async Task<DeleteImageFileResponse> Delete(string publicId)
    {
        var deleteRequest = new DeleteImageFileRequest
        {
            PublicId = publicId
        };
        
        return await _cloudinaryService.DeleteImageFile(deleteRequest);
    }
}