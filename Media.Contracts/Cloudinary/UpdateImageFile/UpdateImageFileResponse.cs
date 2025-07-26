using Platform.Core.Operations;

namespace Media.Contracts.Cloudinary.UpdateImageFile;

public class UpdateImageFileResponse : OperationResponseBase
{
    public string PublicId { get; set; } = string.Empty;
    
    public string Url { get; set; } = string.Empty;
}