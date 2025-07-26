using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza.GetPizzas;
using Catalog.Domain.Pizza;
using Microsoft.EntityFrameworkCore;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Services.Pizza;

internal class PizzaServiceGetPizzasHandler
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public PizzaServiceGetPizzasHandler(IPizzaRepository pizzaRepository, IDbScopeProvider scopeProvider)
    {
        _pizzaRepository = pizzaRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<GetPizzasResponse> GetPizzas(GetPizzasRequest request)
    {
        var response = new GetPizzasResponse();

        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();

        IMapper<PizzaOrm, PizzaDto> mapper = _pizzaRepository.GetMapper();

        List<PizzaOrm> orms = await GetMatchedQueryable(_pizzaRepository.QueryAll(scope), request)
            .Skip(request.Skip ?? 0)
            .Take(request.Take ?? int.MaxValue).ToListAsync();

        response.Ok = true;
        response.Pizzas = orms.ConvertAll(mapper.MapToDto);

        return response;
    }

    private static IQueryable<PizzaOrm> GetMatchedQueryable(
        IQueryable<PizzaOrm> searchQuery,
        GetPizzasRequest request)
    {
        if (request.PizzaId != null)
        {
            searchQuery = searchQuery.Where(x => x.Id == request.PizzaId);
        }

        return searchQuery;
    }
}