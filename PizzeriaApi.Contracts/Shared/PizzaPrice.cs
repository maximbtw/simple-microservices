using Utilities.CustomAttributes;

namespace PizzeriaApi.Contracts.Shared;

public record PizzaPrice([Positive] int Size, [Positive] decimal Price);