using Media.Contracts.Image;
using Media.Contracts.Persistence.Image;

namespace Media.Contracts.Cloudinary.UpdateImageFile;

public class UpdateImageFileRequest
{
    public string PublicId { get; set; } = string.Empty;

    public byte[] FileData { get; set; } = null!;
    
    public string FileName { get; set; } = string.Empty;
    
    public ImageFolder Folder { get; set; }
}