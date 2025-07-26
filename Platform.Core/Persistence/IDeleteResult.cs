namespace Platform.Core.Persistence;

public interface IDeleteResult
{
    bool Ok { get;  }
    
    bool Deleted { get; set; }
}