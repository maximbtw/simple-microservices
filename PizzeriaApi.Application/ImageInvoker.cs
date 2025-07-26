using Media.Contracts.Image;
using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;
using Media.Contracts.Persistence.Image;
using Microsoft.AspNetCore.Http;

namespace PizzeriaApi.Application;

public static class ImageInvoker
{
    public static async Task<UploadImageResponse> UploadImage(IImageService service, IFormFile file, ImageFolder folder)
    {
        var request = new UploadImageRequest
        {
            FileData = await ConvertFromFileToBytesAsync(file),
            Folder = folder
        };

        return await service.UploadImage(request);
    }
    
    public static async Task<DeleteImageResponse> DeleteImage(IImageService service, int imageId)
    {
        var request = new DeleteImageRequest
        {
            ImageId = imageId
        };

        return await service.DeleteImage(request);
    }
    
    public static async Task<UpdateImageResponse> UpdateImage(IImageService service, IFormFile file, int imageId)
    {
        var request = new UpdateImageRequest
        {
            FileData = await ConvertFromFileToBytesAsync(file),
            ImageId = imageId
        };

        return await service.UpdateImage(request);
    }
    
    private static async Task<byte[]> ConvertFromFileToBytesAsync(IFormFile file)
    {
        if (file.Length == 0)
        {
            return [];   
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        
        return memoryStream.ToArray();
    }
}