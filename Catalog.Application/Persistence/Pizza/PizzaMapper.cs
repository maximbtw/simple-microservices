using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza;
using Catalog.Domain.Pizza;
using Platform.Core.Persistence;
using Utilities;

namespace Catalog.Application.Persistence.Pizza;

internal class PizzaMapper : IMapper<PizzaOrm, PizzaDto>
{
    public PizzaDto MapToDto(PizzaOrm orm) => new()
    {
        Id = orm.Id,
        Version = orm.Version,
        Name = orm.Name,
        ImageId = orm.ImageId,
        ImageUrl = orm.ImageUrl,
        Description = orm.Description,
        IsActive = orm.IsActive,
        PizzeriaAccountId = orm.PizzeriaAccountId,
        Prices = orm.Prices.ConvertAll(SizeMapTo),
        IngredientIds = orm.Ingredients.ConvertAll(x => x.IngredientId)
    };

    public void UpdateOrmFromDto(PizzaOrm orm, PizzaDto dto)
    {
        orm.Id = dto.Id;
        orm.Version = dto.Version;
        orm.Name = dto.Name;
        orm.ImageId = dto.ImageId;
        orm.ImageUrl = dto.ImageUrl;
        orm.Description = dto.Description;
        orm.IsActive = dto.IsActive;
        orm.PizzeriaAccountId = dto.PizzeriaAccountId;

        CollectionHelper.Synchronize(
            orm.Prices,
            dto.Prices,
            equals: (o, d) => o.Size == d.Size,
            create: d => new PizzaPriceOrm
            {
                Price = d.Price,
                Size = d.Size
            },
            update: (o, d) =>
            {
                o.Price = d.Price;
                o.Size = d.Size;
            }
        );
        
        CollectionHelper.Synchronize(
            orm.Ingredients,
            dto.IngredientIds,
            equals: (o, d) => o.IngredientId == d,
            create: d => new PizzaIngredientOrm
            {
                IngredientId = d
            },
            update: (o, d) =>
            {
                o.IngredientId = d;
            }
        );
    }

    private static PizzaPriceDto SizeMapTo(PizzaPriceOrm orm) => new()
    {
        Price = orm.Price,
        Size = orm.Size
    };
}