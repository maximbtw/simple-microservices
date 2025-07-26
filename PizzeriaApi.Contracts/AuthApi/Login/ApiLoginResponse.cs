using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.AuthApi.Login;

public class ApiLoginResponse : ApiResponseBase
{
    public string Token { get; set; } = null!;
    
    public AccountUser AccountUser { get; set; } = null!;
}