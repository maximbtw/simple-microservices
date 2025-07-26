namespace Platform.Core.Operations;

public class OperationResponseBase<TErrors> where TErrors : OperationResponseStandardErrors, new()
{
    public bool Ok { get; set; }

    public TErrors Errors { get; set; } = new();
}

public class OperationResponseBase : OperationResponseBase<OperationResponseStandardErrors>
{
}