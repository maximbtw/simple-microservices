using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PizzeriaAccounting.Domain.AccountUser;

internal class AccountUserTypeConfiguration : IEntityTypeConfiguration<AccountUserOrm>
{
    public virtual void Configure(EntityTypeBuilder<AccountUserOrm> builder)
    {
        builder
            .HasOne(d => d.Account)
            .WithMany()
            .HasForeignKey(d => d.AccountId);
    }
}