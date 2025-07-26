using PizzeriaApi.Contracts.AuthApi.Login;
using PizzeriaApi.Contracts.AuthApi.Register;

namespace PizzeriaApi.Contracts.AuthApi;

public interface IApiAuthService
{
    Task<ApiRegisterResponse> Register(ApiRegisterRequest request);
    
    Task<ApiLoginResponse> Login(ApiLoginRequest request);
}