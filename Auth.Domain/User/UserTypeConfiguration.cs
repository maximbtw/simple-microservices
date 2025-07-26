using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Domain.User;

public class UserTypeConfiguration : IEntityTypeConfiguration<UserOrm>
{
    public void Configure(EntityTypeBuilder<UserOrm> builder)
    {
    }
}