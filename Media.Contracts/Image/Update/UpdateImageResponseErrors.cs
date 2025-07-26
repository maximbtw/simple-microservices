using Platform.Core.Operations;

namespace Media.Contracts.Image.Update;

public class UpdateImageResponseErrors : OperationResponseStandardErrors
{
    public bool ImageNotFound { get; set; }
}