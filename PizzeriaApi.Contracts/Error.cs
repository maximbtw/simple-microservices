namespace PizzeriaApi.Contracts;

public record Error(ErrorCode Code, string Message, string? Type = null)
{
    public static Error CreateError(ErrorCode code, string message, string? type = null) => new(code, message, type);
    
    public static Error CreateError(ErrorCode code) => new(code, string.Empty);
    
    public static Error CreateError(string message) => new(ErrorCode.None, message);
}