using Platform.Core.Operations;

namespace Media.Contracts.Cloudinary.UploadImageFile;

public class UploadImageFileResponse : OperationResponseBase
{
    public string Url { get; set; } = string.Empty;
    
    public string PublicId { get; set; } = string.Empty;
}