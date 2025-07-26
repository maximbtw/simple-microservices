using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.CreateIngredient;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Services.Ingredient;

internal class IngredientServiceCreateHandler
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public IngredientServiceCreateHandler(IIngredientRepository ingredientRepository, IDbScopeProvider scopeProvider)
    {
        _ingredientRepository = ingredientRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<CreateIngredientResponse> CreateIngredient(CreateIngredientRequest request)
    {
        var response = new CreateIngredientResponse();

        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        EntityCreateResult<IngredientDto> createResult = await CreateIngredient(request, scope);

        if (createResult.Ok)
        {
            await scope.CommitChangesIfSucceededAsync(createResult.Ok);

            response.Ingredient = createResult.CreatedEntity!;
            response.Ok = true;
        }
        
        return response;
    }

    private async Task<EntityCreateResult<IngredientDto>> CreateIngredient(
        CreateIngredientRequest request,
        OperationModificationScope scope)
    {
        var createRequest = new EntityCreateParameters<IngredientDto>
        {
            Entity = new IngredientDto
            {
                PizzeriaAccountId = request.PizzeriaAccountId,
                ImageId = request.CreateModel.ImageId,
                ImageUrl = request.CreateModel.ImageUrl,
                Name = request.CreateModel.Name,
                IsActive = request.CreateModel.IsActive,
                Price = request.CreateModel.Price
            }
        };

        return await _ingredientRepository.Create(createRequest, scope);
    }
}