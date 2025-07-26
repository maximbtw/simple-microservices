using Media.Contracts.Image;
using Media.Contracts.Persistence.Image;

namespace Media.Contracts.Cloudinary.UploadImageFile;

public class UploadImageFileRequest
{
    public byte[] FileData { get; set; } = null!;
    
    public string FileName { get; set; } = string.Empty;
    
    public ImageFolder Folder { get; set; }
}