using CloudinaryDotNet.Actions;
using Media.Contracts.Cloudinary.DeleteImageFile;

namespace Media.Application.Services.Cloudinary;

public class CloudinaryServiceDeleteImageFileHandler
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryServiceDeleteImageFileHandler(CloudinaryDotNet.Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<DeleteImageFileResponse> DeleteImageFile(DeleteImageFileRequest request)
    {
        var response = new DeleteImageFileResponse();
        
        var deletionParams = new DeletionParams(request.PublicId);
        DeletionResult deleteResult = await _cloudinary.DestroyAsync(deletionParams);
        
        if (deleteResult.Result == "ok")
        {
            response.Ok = true;

            return response;
        }

        if (deleteResult.Result == "not found")
        {
            response.Errors.ImageNotFound = true;
        }
        else
        {
            response.Errors.UnexpectedError = true;
            response.Errors.UnexpectedErrorDescription = deleteResult.Error.Message;
        }

        return response;
    }
}