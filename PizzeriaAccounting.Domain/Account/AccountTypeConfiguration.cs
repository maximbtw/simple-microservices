using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PizzeriaAccounting.Domain.Account;

internal class AccountTypeConfiguration  : IEntityTypeConfiguration<AccountOrm>
{
    public void Configure(EntityTypeBuilder<AccountOrm> builder)
    {
    }
}
