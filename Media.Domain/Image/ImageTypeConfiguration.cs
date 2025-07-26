using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Media.Domain.Image;

internal class ImageTypeConfiguration : IEntityTypeConfiguration<ImageOrm>
{
    public void Configure(EntityTypeBuilder<ImageOrm> builder)
    {
        builder.Property(x => x.Folder).HasConversion<byte>(); 
    }
}