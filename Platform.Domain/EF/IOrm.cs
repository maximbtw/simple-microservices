namespace Platform.Domain.EF;

public interface IOrm
{
    public int Id { get; set; }
    
    public int Version { get; set; }
}