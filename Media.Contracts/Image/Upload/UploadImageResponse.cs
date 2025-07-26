using Media.Contracts.Persistence.Image;
using Platform.Core.Operations;

namespace Media.Contracts.Image.Upload;

public class UploadImageResponse : OperationResponseBase
{
    public ImageDto Image { get; set; } = null!;
}