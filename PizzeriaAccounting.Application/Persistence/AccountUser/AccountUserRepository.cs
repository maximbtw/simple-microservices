using Microsoft.EntityFrameworkCore;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using PizzeriaAccounting.Domain.AccountUser;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Persistence.AccountUser;

internal class AccountUserRepository : EntityRepositoryBase<
        AccountUserOrm,
        AccountUserDto,
        CreateAccountUserParameters,
        CreateAccountUserRResult,
        EntityUpdateParameters<AccountUserDto>,
        EntityUpdateResult<AccountUserDto>,
        EntityDeleteParameters,
        EntityDeleteResult>,
    IAccountUserRepository
{
    public override AccountUserMapper GetMapper() => new();

    public async Task<AccountUserDto?> GetOrNullByUserId(int userId, OperationScopeBase scope)
    {
        AccountUserMapper mapper = GetMapper();

        AccountUserOrm? orm = await QueryAll(scope).FirstOrDefaultAsync(x => x.UserId == userId);
        
        return orm == null ? null : mapper.MapToDto(orm);
    }
}