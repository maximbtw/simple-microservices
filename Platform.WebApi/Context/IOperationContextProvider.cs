namespace Platform.WebApi.Context;

public interface IOperationContextProvider
{
    OperationContext? GetContext();

    void SetOperationContext(OperationContext operationContext);
}