using PizzeriaAccounting.Contracts.Persistence.Account;
using PizzeriaAccounting.Domain.Account;
using Platform.Core.Persistence;

namespace PizzeriaAccounting.Application.Persistence.Account;

internal class AccountMapper : IMapper<AccountOrm, AccountDto>
{
    public AccountDto MapToDto(AccountOrm orm) => new()
    {
        Id = orm.Id,
        Version = orm.Version,
        Name = orm.Name,
        Address = orm.Address,
        Email = orm.Email,
        Phone = orm.Phone,
        IsActive = orm.IsActive
    };

    public void UpdateOrmFromDto(AccountOrm orm, AccountDto dto)
    {
        orm.Id = dto.Id;
        orm.Version = dto.Version;
        orm.Name = dto.Name;
        orm.Address = dto.Address;
        orm.Email = dto.Email;
        orm.Phone = dto.Phone;
        orm.IsActive = dto.IsActive;
    }
}