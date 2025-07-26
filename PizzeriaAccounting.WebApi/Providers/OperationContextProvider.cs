using Platform.WebApi.Context;

namespace PizzeriaAccounting.WebApi.Providers;

internal class OperationContextProvider : IOperationContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private OperationContext _operationContext = null!;

    public OperationContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Не работает с синглоном
    public OperationContext? GetContext()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return _operationContext;
        }
        
        return (OperationContext)_httpContextAccessor.HttpContext.Items[nameof(OperationContext)]!;
    }

    public void SetOperationContext(OperationContext operationContext)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _operationContext = operationContext;
        }
        else
        {
            _httpContextAccessor.HttpContext.Items[nameof(OperationContext)] = operationContext;
        }
    }
}