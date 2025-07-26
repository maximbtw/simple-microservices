using Media.Application.Persistence.Image;
using Media.Contracts.Cloudinary;
using Media.Contracts.Cloudinary.UpdateImageFile;
using Media.Contracts.Image;
using Media.Contracts.Image.Update;
using Media.Contracts.Persistence.Image;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Utilities;

namespace Media.Application.Services.Image;

internal class ImageServiceUpdateImageHandler
{
    private readonly IImageRepository _imageRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IDbScopeProvider _scopeProvider;

    public ImageServiceUpdateImageHandler(
        IImageRepository imageRepository, 
        ICloudinaryService cloudinaryService,
        IDbScopeProvider scopeProvider)
    {
        _imageRepository = imageRepository;
        _cloudinaryService = cloudinaryService;
        _scopeProvider = scopeProvider;
    }

    public async Task<UpdateImageResponse> UpdateImage(UpdateImageRequest request)
    {
        var response = new UpdateImageResponse();

        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        ImageDto? image = await _imageRepository.GetOrNull(request.ImageId, scope);
        if (image == null)
        {
            response.Errors.ImageNotFound = true;
            return response;
        }

        UpdateImageFileResponse updateImageFileResponse = await UpdateImageFile(request, image);
        if (!updateImageFileResponse.Ok)
        {
            return response;
        }

        EntityUpdateResult<ImageDto> updateResult = await UpdateImageModel(
            image, 
            updateImageFileResponse.PublicId,
            updateImageFileResponse.Url,
            scope);

        if (updateResult.Ok)
        {
            await scope.CommitChangesIfSucceededAsync(updateResult.Ok);
            
            response.ImageUrl = updateImageFileResponse.Url;
            response.Ok = true;
        }
        
        return response;
    }

    private async Task<UpdateImageFileResponse> UpdateImageFile(UpdateImageRequest request, ImageDto image)
    {
        var updateImageFileRequest = new UpdateImageFileRequest
        {
            PublicId = image.PublicId,
            Folder = image.Folder,
            FileData = request.FileData,
            FileName = image.Id.ToString()
        };
        
        return await _cloudinaryService.UpdateImageFile(updateImageFileRequest);
    }

    private async Task<EntityUpdateResult<ImageDto>> UpdateImageModel(
        ImageDto image,
        string publicId,
        string url,
        OperationModificationScope scope)
    {
        ImageDto modifiedImage = CloneHelper.CloneDeep(image);
        
        modifiedImage.PublicId = publicId;
        modifiedImage.Url = url;

        var parameters = new EntityUpdateParameters<ImageDto>
        {
            Entity = modifiedImage
        };

        return await _imageRepository.Update(parameters, scope);
    }
}