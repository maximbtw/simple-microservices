namespace PizzeriaApi.Contracts;

public enum ErrorCode
{    
    None = 0,
    ServerError,
    NotAuthorized,
    AccessDenied,
    NotFound,
    Conflict
}