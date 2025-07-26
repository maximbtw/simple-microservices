using Microsoft.EntityFrameworkCore;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Persistence.Account;
using PizzeriaAccounting.Domain.Account;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Persistence.Account;

internal class AccountRepository : EntityRepositoryBase<
        AccountOrm,
        AccountDto,
        CreateAccountParameters,
        CreateAccountResult,
        UpdateAccountParameters,
        UpdateAccountResult,
        EntityDeleteParameters,
        EntityDeleteResult>,
    IAccountRepository
{
    public override AccountMapper GetMapper() => new();
    
    protected override async Task<bool> OnEntityCreating(
        CreateAccountParameters parameters,
        CreateAccountResult result,
        OperationModificationScope scope)
    {
        (bool SameNameExists, bool SameAddressExists) conflicts =
            await TryGetUpdateOrCreateConflicts(parameters.Entity, scope);

        bool hasConflict = 
            conflicts.SameNameExists || conflicts.SameAddressExists;

        if (hasConflict)
        {
            result.SameAddressExists = conflicts.SameAddressExists;
            result.SameNameExists = conflicts.SameNameExists;
            
            return false;
        }

        return await base.OnEntityCreating(parameters, result, scope);
    }

    protected override async Task<bool> OnEntityUpdating(
        UpdateAccountParameters parameters, 
        UpdateAccountResult result,
        AccountDto oldModel,
        OperationModificationScope scope)
    {
        (bool SameNameExists, bool SameAddressExists) conflicts =
            await TryGetUpdateOrCreateConflicts(parameters.Entity, scope);

        bool hasConflict = 
            conflicts.SameNameExists || conflicts.SameAddressExists;

        if (hasConflict)
        {
            result.SameAddressExists = conflicts.SameAddressExists;
            result.SameNameExists = conflicts.SameNameExists;
            
            return false;
        }
        
        return await base.OnEntityUpdating(parameters, result, oldModel, scope);
    }

    private async Task<(bool SameNameExists, bool SameAddressExists)> TryGetUpdateOrCreateConflicts(
        AccountDto model, 
        OperationModificationScope scope)
    {
        IQueryable<AccountOrm> query = QueryAll(scope);
        
        bool sameNameExists = await query.AnyAsync(x => x.Name.ToUpper() == model.Name.ToUpper());
        bool sameAddressExists = await query.AnyAsync(x => x.Address.ToUpper() == model.Address.ToUpper());

        return (sameNameExists, sameAddressExists);
    }

    public async Task<List<AccountDto>> GetAll(OperationReaderScope scope)
    {
        AccountMapper mapper = GetMapper();
        List<AccountOrm> orms = await QueryAll(scope).ToListAsync();

        return orms.ConvertAll(mapper.MapToDto);
    }
}