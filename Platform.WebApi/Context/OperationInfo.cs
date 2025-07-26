namespace Platform.WebApi.Context;

public class OperationInfo
{
    public string OperationName { get; set; } = string.Empty;
    
    public DateTime OperationStartDateTimeUtc { get; set; }
}