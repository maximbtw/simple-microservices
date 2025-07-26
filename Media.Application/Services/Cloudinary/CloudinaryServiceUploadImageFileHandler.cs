using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Media.Contracts.Cloudinary.UploadImageFile;
using Utilities;

namespace Media.Application.Services.Cloudinary;

internal class CloudinaryServiceUploadImageFileHandler
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;
    
    public CloudinaryServiceUploadImageFileHandler(CloudinaryDotNet.Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<UploadImageFileResponse> UpdateImageFile(UploadImageFileRequest request)
    {
        if (CollectionHelper.IsNullOrEmpty(request.FileData))
        {
            return new UploadImageFileResponse
            {
                Ok = false
            };
        }

        await using Stream stream = new MemoryStream(request.FileData);
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.FileName, stream),
            Folder = request.Folder.ToString()
        };

        ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return new UploadImageFileResponse
            {
                Ok = false
            };
        }

        return new UploadImageFileResponse
        {
            Ok = true,
            Url = uploadResult.SecureUrl!.ToString(),
            PublicId = uploadResult.PublicId
        };
    }
}