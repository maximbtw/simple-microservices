using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Contracts.Persistence.Pizza;
using Platform.Core.Operations;

namespace Catalog.Contracts.Pizza.GetPizzaOptions;

public class GetPizzaOptionsResponse : OperationResponseBase<GetPizzaOptionsResponseErrors>
{
    public PizzaDto? Pizza { get; set; }

    public List<IngredientDto>? Ingredients { get; set; }
    
    public List<IngredientDto> AvailableIngredients { get; set; } = new();
}