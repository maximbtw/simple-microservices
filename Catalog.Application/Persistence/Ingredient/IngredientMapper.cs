using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Core.Persistence;

namespace Catalog.Application.Persistence.Ingredient;

internal class IngredientMapper : IMapper<IngredientOrm, IngredientDto>
{
    public IngredientDto MapToDto(IngredientOrm orm) => new()
    {
        Id = orm.Id,
        Version = orm.Version,
        Name = orm.Name,
        ImageId = orm.ImageId,
        ImageUrl = orm.ImageUrl,
        IsActive = orm.IsActive,
        PizzeriaAccountId = orm.PizzeriaAccountId,
        Price = orm.Price,
    };

    public void UpdateOrmFromDto(IngredientOrm orm, IngredientDto dto)
    {
        orm.Id = dto.Id;
        orm.Version = dto.Version;
        orm.Name = dto.Name;
        orm.ImageId = dto.ImageId;
        orm.ImageUrl = dto.ImageUrl;
        orm.IsActive = dto.IsActive;
        orm.Price = dto.Price;
        orm.PizzeriaAccountId = dto.PizzeriaAccountId;
    }
}