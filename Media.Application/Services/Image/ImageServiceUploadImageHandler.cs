using Media.Application.Persistence.Image;
using Media.Contracts.Cloudinary;
using Media.Contracts.Cloudinary.UploadImageFile;
using Media.Contracts.Image;
using Media.Contracts.Image.Upload;
using Media.Contracts.Persistence.Image;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Media.Application.Services.Image;

internal class ImageServiceUploadImageHandler
{
    private readonly IImageRepository _imageRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IDbScopeProvider _scopeProvider;

    public ImageServiceUploadImageHandler(
        IImageRepository imageRepository,
        ICloudinaryService cloudinaryService,
        IDbScopeProvider scopeProvider)
    {
        _imageRepository = imageRepository;
        _cloudinaryService = cloudinaryService;
        _scopeProvider = scopeProvider;
    }

    public async Task<UploadImageResponse> UploadImage(UploadImageRequest request)
    {
        var response = new UploadImageResponse();

        var uploadRequest = new UploadImageFileRequest
        {
            FileData = request.FileData,
            FileName = Guid.NewGuid().ToString(),
            Folder = request.Folder,
        };

        UploadImageFileResponse uploadImageFileResponse = await _cloudinaryService.UploadImageFile(uploadRequest);
        if (!uploadImageFileResponse.Ok)
        {
            return response;
        }

        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        var parameters = new EntityCreateParameters<ImageDto> 
        {
            Entity = new ImageDto
            {
                Url = uploadImageFileResponse.Url,
                PublicId = uploadImageFileResponse.PublicId
            }
        };

        EntityCreateResult<ImageDto> createResult = await _imageRepository.Create(parameters, scope);
        if (createResult.Ok)
        {
            await scope.CommitChangesIfSucceededAsync(createResult.Ok);

            response.Ok = createResult.Ok;
            response.Image = createResult.CreatedEntity!;
        }

        return response;
    }
}