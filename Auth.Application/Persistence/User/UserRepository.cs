using System.Text;
using Auth.Contracts.Persistence.User;
using Auth.Domain.User;
using Microsoft.EntityFrameworkCore;
using Platform.Core.Persistence;
using Platform.Domain.EF.Transactions;

namespace Auth.Application.Persistence.User;

internal class UserRepository : EntityRepositoryBase<
        UserOrm,
        UserDto,
        CreateUserParameters,
        CreateUserResult,
        EntityUpdateParameters<UserDto>,
        EntityUpdateResult<UserDto>,
        EntityDeleteParameters,
        EntityDeleteResult>,
    IUserRepository
{
    public override UserMapper GetMapper() => new();

    public async Task<UserDto?> GetOrNull(string login, string password, OperationScopeBase scope)
    {
        IMapper<UserOrm, UserDto> mapper = GetMapper();

        byte[] passwordHash = Encoding.UTF8.GetBytes(password);

        UserOrm? userOrm = await QueryAll(scope)
            .FirstOrDefaultAsync(x => x.Login == login && x.PasswordHash == passwordHash);
        
        return userOrm == null ? null : mapper.MapToDto(userOrm);
    }

    protected override async Task<bool> OnEntityCreating(
        CreateUserParameters parameters,
        CreateUserResult result,
        OperationModificationScope scope)
    {
        bool userExists = await QueryAll(scope).AnyAsync(x => x.Login == parameters.Entity.Login);
        if (userExists)
        {
            result.UserWithSameLoginExists = true;

            return false;
        }

        return true;
    }

    protected override Task OnEntityCreated(
        CreateUserParameters parameters, 
        CreateUserResult result,
        OperationModificationScope scope)
    {
        result.CreatedEntity!.PasswordHash = null;

        return base.OnEntityCreated(parameters, result, scope);
    }
}