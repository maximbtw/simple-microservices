using Catalog.Contracts.Persistence.Pizza;
using Catalog.Domain.Pizza;
using Platform.Core.Persistence;

namespace Catalog.Application.Persistence.Pizza;

internal class PizzaRepository : EntityRepositoryBase<
            PizzaOrm,
            PizzaDto,
            EntityCreateParameters<PizzaDto>,
            EntityCreateResult<PizzaDto>,
            EntityUpdateParameters<PizzaDto>,
            EntityUpdateResult<PizzaDto>,
            EntityDeleteParameters,
            EntityDeleteResult>,
        IPizzaRepository
{
    public override PizzaMapper GetMapper() => new();
}