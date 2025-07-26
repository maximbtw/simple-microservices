using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using PizzeriaAccounting.Domain.AccountUser;
using Platform.Core.Persistence;

namespace PizzeriaAccounting.Application.Persistence.AccountUser;

internal class AccountUserMapper : IMapper<AccountUserOrm, AccountUserDto>
{
    public AccountUserDto MapToDto(AccountUserOrm orm) => new()
    {
        Id = orm.Id,
        Version = orm.Version,
        AccountId = orm.AccountId,
        Email = orm.Email,
        IsActive = orm.IsActive,
        UserId = orm.UserId
    };

    public void UpdateOrmFromDto(AccountUserOrm orm, AccountUserDto dto)
    {
        orm.Id = dto.Id;
        orm.Version = dto.Version;
        orm.AccountId = dto.AccountId;
        orm.Email = dto.Email;
        orm.IsActive = dto.IsActive;
        orm.UserId = dto.UserId;
    }
}