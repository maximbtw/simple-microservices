using Media.Contracts.Persistence.Image;

namespace Media.Contracts.Image.Upload;

public class UploadImageRequest
{
    public byte[] FileData { get; set; } = null!;
    
    public ImageFolder Folder { get; set; }
}