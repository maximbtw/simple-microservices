using System.ComponentModel.DataAnnotations.Schema;
using Media.Contracts.Persistence.Image;
using Platform.Domain.EF;

namespace Media.Domain.Image;

[Table("Images")]
public class ImageOrm : IOrm
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Url { get; set; } = string.Empty;

    public string PublicId { get; set; } = string.Empty;

    public ImageFolder Folder { get; set; }
}