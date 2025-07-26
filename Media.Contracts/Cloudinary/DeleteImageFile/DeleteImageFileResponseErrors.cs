using Platform.Core.Operations;

namespace Media.Contracts.Cloudinary.DeleteImageFile;

public class DeleteImageFileResponseErrors : OperationResponseStandardErrors
{
    public bool ImageNotFound { get; set; }
    
    public bool UnexpectedError  { get; set; }
    
    public string UnexpectedErrorDescription { get; set; } = string.Empty;
}