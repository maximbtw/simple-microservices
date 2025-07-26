namespace PizzeriaApi.Contracts;

public class ApiResponseBase
{
    private List<Error> _errors { get; } = new();

    public bool Ok => _errors.Count == 0;
    
    public List<Error> Errors => _errors;

    public Error? Error => _errors.OrderBy(x => x.Code).FirstOrDefault();

    public void AddError(Error error)
    {
        _errors.Add(error);
    }
    
    public ApiResponseBase AddErrorIf( bool condition, ErrorCode code, string message, Enum? type = null)
    {
        if (condition)
        {
            AddError(Error.CreateError(code, message, type?.ToString() ?? null));   
        }
        
        return this;
    }
    
    public ApiResponseBase AddErrorIf( bool condition, ErrorCode code)
    {
        if (condition)
        {
            AddError(Error.CreateError(code));   
        }
        
        return this;
    }
}