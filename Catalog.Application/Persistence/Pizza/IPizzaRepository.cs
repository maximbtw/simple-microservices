using Catalog.Domain.Pizza;
using Platform.Core.Persistence;
using Platform.Domain.EF.Transactions;

namespace Catalog.Contracts.Persistence.Pizza;

public interface IPizzaRepository
{
    IQueryable<PizzaOrm> QueryAll(OperationScopeBase scope);
    
    Task<PizzaDto?> GetOrNull(int id, OperationScopeBase scope);

    Task<EntityCreateResult<PizzaDto>> Create(
        EntityCreateParameters<PizzaDto> parameters,
        OperationModificationScope scope);

    Task<EntityUpdateResult<PizzaDto>> Update(
        EntityUpdateParameters<PizzaDto> parameters,
        OperationModificationScope scope);

    IMapper<PizzaOrm, PizzaDto> GetMapper();
}