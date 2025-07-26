namespace Platform.Core.Persistence;

public interface IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }
}