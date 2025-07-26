using Media.Contracts.Image;
using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;
using Microsoft.AspNetCore.Mvc;
using Platform.Domain;
using Platform.WebApi;

namespace Media.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController(IImageService service) : ControllerBase, IImageService
{
    [RequireRoles(UserRole.PizzeriaAccountUser, UserRole.Admin)]
    [HttpPost("[action]")]
    public async Task<UploadImageResponse> UploadImage(UploadImageRequest request) =>
        await service.UploadImage(request);

    [RequireRoles(UserRole.PizzeriaAccountUser, UserRole.Admin)]
    [HttpPost("[action]")]
    public async Task<UpdateImageResponse> UpdateImage(UpdateImageRequest request) =>
        await service.UpdateImage(request);

    [RequireRoles(UserRole.PizzeriaAccountUser, UserRole.Admin)]
    [HttpPost("[action]")]
    public async Task<DeleteImageResponse> DeleteImage(DeleteImageRequest request) =>
        await service.DeleteImage(request);
}