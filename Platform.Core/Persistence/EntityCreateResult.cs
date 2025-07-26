namespace Platform.Core.Persistence;

public class EntityCreateResult<TDto> : ICreateResult<TDto> where TDto : IEntityDto
{
    public virtual bool Ok => this.CreatedEntity != null;
    
    public TDto? CreatedEntity { get; set; }
}