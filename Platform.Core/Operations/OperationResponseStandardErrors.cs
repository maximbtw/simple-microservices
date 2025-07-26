namespace Platform.Core.Operations;

public class OperationResponseStandardErrors
{
    public bool AccessDenied { get; set; }
    
    public AccessDeniedReason? AccessDeniedReason { get; set; }
    
    public bool InternalServerError { get; set; }
    
    public bool InvalidRequest { get; set; }
    
    public string? InvalidRequestDescription { get; set; } 
}