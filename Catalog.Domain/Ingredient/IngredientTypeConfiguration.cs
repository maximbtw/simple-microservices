using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Domain.Ingredient;

internal class IngredientTypeConfiguration : IEntityTypeConfiguration<IngredientOrm>
{
    public void Configure(EntityTypeBuilder<IngredientOrm> builder)
    {
    }
}