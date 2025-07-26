namespace Media.Contracts.Image.Update;

public class UpdateImageRequest
{
    public int ImageId { get; set; }

    public byte[] FileData { get; set; } = null!;
}