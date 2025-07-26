using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Utilities.CustomExceptions;

namespace Platform.Core.Persistence;

public abstract class EntityRepositoryBase<
    TOrm,
    TDto,
    TCreateParameters,
    TCreateResult,
    TUpdateParameters,
    TUpdateResult,
    TDeleteParameters,
    TDeleteResult>
    where TOrm : class, IOrm, new()
    where TDto : class, IEntityDto, new()
    where TCreateParameters : ICreateParameters<TDto>
    where TCreateResult : ICreateResult<TDto>, new()
    where TUpdateParameters : IUpdateParameters<TDto>
    where TUpdateResult : IUpdateResult<TDto>, new()
    where TDeleteParameters : IDeleteParameters
    where TDeleteResult : IDeleteResult, new()
{
    public virtual async Task<TDto> Get(int id, OperationScopeBase scope)
    {
        DbContextBase dbContext = scope.GetDbContext();
        IMapper<TOrm, TDto> mapper = GetMapper();
        
        TOrm? orm = await GetOrNull(id, dbContext);
        if (orm == null)
        {
            throw new EntityNotFoundException(typeof(TOrm), id);
        }

        return mapper.MapToDto(orm);
    }
    
    public virtual async Task<TDto?> GetOrNull(int id, OperationScopeBase scope)
    {
        DbContextBase dbContext = scope.GetDbContext();
        IMapper<TOrm, TDto> mapper = GetMapper();
        
        TOrm? orm = await GetOrNull(id, dbContext);
        if (orm == null)
        {
            return null;
        }

        return mapper.MapToDto(orm);
    }

    public virtual IQueryable<TOrm> QueryAll(OperationScopeBase scope)
    {
        DbContextBase dbContext = scope.GetDbContext();
        
        return dbContext.Set<TOrm>().AsNoTracking();
    }

    public virtual async Task<TCreateResult> Create(TCreateParameters parameters, OperationModificationScope scope)
    {
        var result = new TCreateResult();
        
        if (parameters.Entity.Id != 0)
        {
            throw new InvalidOperationException("Model id must be 0");
        }
        
        IMapper<TOrm, TDto> mapper = GetMapper();
        
        var orm = new TOrm();
        if (await OnEntityCreating(parameters, result, scope))
        {
            parameters.Entity.Id = 0;

            mapper.UpdateOrmFromDto(orm, parameters.Entity);
            
            DbContextBase dbContext = scope.GetDbContext();
        
            EntityEntry<TOrm> addResult = await dbContext.AddAsync(orm);

            await scope.SaveAsync();
            
            result.CreatedEntity = mapper.MapToDto(addResult.Entity);
        
            await OnEntityCreated(parameters, result, scope);
        }

        return result;
    }

    public virtual async Task<TUpdateResult> Update(TUpdateParameters parameters, OperationModificationScope scope)
    {
        var result = new TUpdateResult();
        
        DbContextBase dbContext = scope.GetDbContext();
        IMapper<TOrm, TDto> mapper = GetMapper();

        TOrm? orm = await dbContext.Set<TOrm>().FirstOrDefaultAsync(x => x.Id == parameters.Entity.Id);
        if (orm == null)
        {
            throw new EntityNotFoundException(typeof(TOrm), parameters.Entity.Id);
        }

        if (orm.Version != parameters.Entity.Version)
        {
            result.VersionConflict = true;
            return result;
        }
        
        TDto oldModel = mapper.MapToDto(orm);
        if (await OnEntityUpdating(parameters, result, oldModel, scope))
        {
            mapper.UpdateOrmFromDto(orm, parameters.Entity);
            
            orm.Version++;
            
            EntityEntry<TOrm> updateResult = dbContext.Update(orm);
        
            await scope.SaveAsync();
            
            result.UpdatedEntity = mapper.MapToDto(updateResult.Entity);
            
            await OnEntityUpdated(parameters, result, scope);
        }

        return result;
    }

    public virtual async Task<TDeleteResult> Delete(TDeleteParameters parameters, OperationModificationScope scope)
    {
        var result = new TDeleteResult();
        if (await OnEntityDeleting(parameters, result, scope))
        {
            DbContextBase dbContext = scope.GetDbContext();

            TOrm? entity = await GetOrNull(parameters.EntityId, dbContext);
            if (entity == null)
            {
                return result;
            }

            dbContext.Remove(entity);
        
            await scope.SaveAsync();
            
            await OnEntityDeleted(parameters, result, scope);
            
            result.Deleted = true;
        }

        return result;
    }

    public abstract IMapper<TOrm, TDto> GetMapper();

    protected virtual Task OnEntityCreated(
        TCreateParameters parameters, 
        TCreateResult result,
        OperationModificationScope scope)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<bool> OnEntityCreating(
        TCreateParameters parameters,
        TCreateResult result,
        OperationModificationScope scope)
    {
        return Task.FromResult(true);
    }
    
    protected virtual Task OnEntityUpdated(
        TUpdateParameters parameters, 
        TUpdateResult result, 
        OperationModificationScope scope)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<bool> OnEntityUpdating(
        TUpdateParameters parameters, 
        TUpdateResult result,
        TDto oldModel,
        OperationModificationScope scope)
    {
        return Task.FromResult(true);
    }
    
    protected virtual Task OnEntityDeleted(
        TDeleteParameters parameters, 
        TDeleteResult result, 
        OperationModificationScope scope)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<bool> OnEntityDeleting(
        TDeleteParameters parameters, 
        TDeleteResult result,
        OperationModificationScope scope)
    {
        return Task.FromResult(true);
    }
    
    private async Task<TOrm?> GetOrNull(int id, DbContextBase dbContext)
    {
        return await dbContext.Set<TOrm>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}