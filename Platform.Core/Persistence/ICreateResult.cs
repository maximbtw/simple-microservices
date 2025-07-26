namespace Platform.Core.Persistence;

public interface ICreateResult<TDto> where TDto : IEntityDto
{
    bool Ok { get; }
    
    TDto? CreatedEntity { get; set; }
}