namespace Platform.Core.Persistence;

public class EntityDeleteResult : IDeleteResult
{
    public virtual bool Ok => true;
    
    public bool Deleted { get; set; }
}