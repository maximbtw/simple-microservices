namespace Platform.WebApi.Context;

public class OperationContext
{
    public ICurrentUser? User { get; set; }
    
    public string TraceId { get; set; } = string.Empty;

    public OperationInfo OperationInfo { get; set; } = new();
}