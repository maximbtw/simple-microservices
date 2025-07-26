using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.Account;

public class UpdateAccountResult : IUpdateResult<AccountDto>
{
    public bool Ok => !this.SameAddressExists
                      && !this.SameNameExists
                      && this.UpdatedEntity != null
                      && !this.VersionConflict;

    public AccountDto? UpdatedEntity { get; set; } = null!;

    public bool VersionConflict { get; set; }

    public bool SameAddressExists { get; set; }

    public bool SameNameExists { get; set; }
}