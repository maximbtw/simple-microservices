using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Domain.Pizza;

internal class PizzaTypeConfiguration : IEntityTypeConfiguration<PizzaOrm>
{
    public void Configure(EntityTypeBuilder<PizzaOrm> builder)
    {
        builder.OwnsMany(p => p.Prices, ConfigureSizes);
        builder.OwnsMany(p => p.Ingredients, ConfigureIngredients);
    }

    private void ConfigureSizes(OwnedNavigationBuilder<PizzaOrm, PizzaPriceOrm> builder)
    {
        builder.WithOwner().HasForeignKey(i => i.PizzaId);
        
        builder.HasKey(i => new { i.PizzaId, i.Size });
    }
    
    private void ConfigureIngredients(OwnedNavigationBuilder<PizzaOrm, PizzaIngredientOrm> builder)
    {
        builder.WithOwner().HasForeignKey(i => i.PizzaId);
        
        builder.HasKey(i => new { i.PizzaId, i.IngredientId });
    }
}