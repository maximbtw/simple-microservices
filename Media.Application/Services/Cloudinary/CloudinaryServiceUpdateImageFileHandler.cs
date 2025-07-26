using Media.Contracts.Cloudinary;
using Media.Contracts.Cloudinary.DeleteImageFile;
using Media.Contracts.Cloudinary.UpdateImageFile;
using Media.Contracts.Cloudinary.UploadImageFile;

namespace Media.Application.Services.Cloudinary;

internal class CloudinaryServiceUpdateImageFileHandler
{
    private readonly ICloudinaryService _cloudinaryService;

    public CloudinaryServiceUpdateImageFileHandler(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    public async Task<UpdateImageFileResponse> UpdateImageFile(UpdateImageFileRequest request)
    {
        var response = new UpdateImageFileResponse();
        
        DeleteImageFileResponse deleteResponse = await Delete(request);
        if (!deleteResponse.Ok && !deleteResponse.Errors.ImageNotFound)
        {
            return response;
        }

        UploadImageFileResponse uploadResponse = await Update(request);
        
        response.Ok = uploadResponse.Ok;
        response.PublicId = uploadResponse.PublicId;
        response.Url = uploadResponse.Url;

        return response;
    }

    private async Task<DeleteImageFileResponse> Delete(UpdateImageFileRequest request)
    {
        var deleteRequest = new DeleteImageFileRequest
        {
            PublicId = request.PublicId
        };
        
        return await _cloudinaryService.DeleteImageFile(deleteRequest);
    }
    
    private async Task<UploadImageFileResponse> Update(UpdateImageFileRequest request)
    {
        var uploadRequest = new UploadImageFileRequest
        {
            FileData = request.FileData,
            Folder = request.Folder,
            FileName = request.FileName
        };
        
        return await _cloudinaryService.UploadImageFile(uploadRequest);
    }
}