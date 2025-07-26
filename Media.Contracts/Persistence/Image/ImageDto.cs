using Platform.Core.Persistence;

namespace Media.Contracts.Persistence.Image;

public class ImageDto : IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Url { get; set; } = string.Empty;
    
    public string PublicId { get; set; }= string.Empty;
    
    public ImageFolder Folder { get; set; }
}