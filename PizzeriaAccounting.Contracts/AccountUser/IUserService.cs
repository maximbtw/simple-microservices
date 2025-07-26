using PizzeriaAccounting.Contracts.AccountUser.Activate;
using PizzeriaAccounting.Contracts.AccountUser.Deactivate;
using PizzeriaAccounting.Contracts.AccountUser.GetUser;
using PizzeriaAccounting.Contracts.AccountUser.Login;
using PizzeriaAccounting.Contracts.AccountUser.Register;

namespace PizzeriaAccounting.Contracts.AccountUser;

public interface IUserService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request);
    
    Task<LoginUserResponse> Login(LoginUserRequest request);
    
    Task<GetUserResponse> GetUser(GetUserRequest request);
    
    Task<ActivateUserResponse> Activate(ActivateUserRequest request);
    
    Task<DeactivateUserResponse> Deactivate(DeactivateUserRequest request);
}