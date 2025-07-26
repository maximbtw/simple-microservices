using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Login;
using PizzeriaAccounting.Contracts.AccountUser.Register;
using PizzeriaApi.Application.Resources;
using PizzeriaApi.Contracts;
using PizzeriaApi.Contracts.AuthApi;
using PizzeriaApi.Contracts.AuthApi.Login;
using PizzeriaApi.Contracts.AuthApi.Register;
using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Application.Services;

internal class ApiAuthService : IApiAuthService
{
    private readonly IUserService _userService;

    public ApiAuthService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ApiRegisterResponse> Register(ApiRegisterRequest request)
    {
        var serviceRequest = new RegisterUserRequest
        {
            AccountId = request.AccountId,
            Email = request.Email,
            Login = request.Login,
            Password = request.Password
        };

        return await OperationInvoker
            .InvokeServiceAsync<RegisterUserResponse, RegisterUserResponseErrors, ApiRegisterResponse>(
                () => _userService.Register(serviceRequest),
                onFailed: (s, apiResponse) => MapErrors(s.Errors, apiResponse));

        void MapErrors(RegisterUserResponseErrors e, ApiRegisterResponse apiResponse)
        {
            apiResponse
                .AddErrorIf(e.AccountNotActive, ErrorCode.AccessDenied, Res.Register_Error_AccountNotActive)
                .AddErrorIf(e.AccountNotFound, ErrorCode.NotFound, Res.Register_Error_AccountNotFound)
                .AddErrorIf(
                    e.UserWithSameEmailExists,
                    ErrorCode.Conflict,
                    Res.Register_Error_UserWithSameEmailExists,
                    type: ApiRegisterResponseErrors.UserWithSameEmailExists)
                .AddErrorIf(
                    e.UserWithSameLoginExists,
                    ErrorCode.Conflict,
                    Res.Register_Error_UserWithSameLoginExists,
                    type: ApiRegisterResponseErrors.UserWithSameLoginExists);
        }
    }

    public async Task<ApiLoginResponse> Login(ApiLoginRequest request)
    {
        var serviceRequest = new LoginUserRequest
        {
            Login = request.Login,
            Password = request.Password
        };

        return await OperationInvoker
            .InvokeServiceAsync<LoginUserResponse, LoginUserResponseErrors, ApiLoginResponse>(
                () => _userService.Login(serviceRequest),
                onFailed: (s, apiResponse) => MapErrors(s.Errors, apiResponse),
                onSuccess: (s, apiResponse) =>
                {
                    apiResponse.Token = s.Token;
                    apiResponse.AccountUser = new AccountUser(
                        s.User.Id,
                        s.User.AccountId,
                        request.Login,
                        s.User.Email);
                });

        void MapErrors(LoginUserResponseErrors e, ApiLoginResponse apiResponse)
        {
            apiResponse
                .AddErrorIf(
                    e.InvalidPasswordOrLogin, 
                    ErrorCode.Conflict, 
                    Res.Login_Error_InvalidPasswordOrLogin,
                    type: ApiAuthResponseErrors.InvalidPasswordOrLogin)
                .AddErrorIf(e.AccessDenied, ErrorCode.Conflict, Res.Login_Error_UserNotActive);
        }
    }
}