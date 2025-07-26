using Platform.Core.Operations;

namespace Media.Contracts.Image.Update;

public class UpdateImageResponse : OperationResponseBase<UpdateImageResponseErrors>
{
    public string ImageUrl { get; set; } = string.Empty;
}