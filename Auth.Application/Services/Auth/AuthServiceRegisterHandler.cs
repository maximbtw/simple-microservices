using System.Text;
using Auth.Application.Persistence.User;
using Auth.Contracts.Auth.Register;
using Auth.Contracts.Persistence.User;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Utilities;

namespace Auth.Application.Services.Auth;

internal class AuthServiceRegisterHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public AuthServiceRegisterHandler(
        IUserRepository userRepository,
        IDbScopeProvider scopeProvider)
    {
        _userRepository = userRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var response = new RegisterResponse();
        
        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();
        
        CreateUserResult createUserResult = await CreateUser(request, scope);
        if (!createUserResult.Ok)
        {
            response.Errors.LoginExists = createUserResult.UserWithSameLoginExists;
            
            return response;
        }
        
        await scope.CommitChangesIfSucceededAsync(createUserResult.Ok);

        response.User = createUserResult.CreatedEntity!;
        response.Ok = true;

        return response;
    }

    private async Task<CreateUserResult> CreateUser(
        RegisterRequest request,
        OperationModificationScope scope)
    {
        var parameters = new CreateUserParameters
        {
            Entity = new UserDto
            {
                Login = request.Login,
                Role = request.Role,
                PasswordHash = Encoding.UTF8.GetBytes(request.Password)
            }
        };

        CreateUserResult createResult = await _userRepository.Create(parameters, scope);

        return createResult;
    }
}