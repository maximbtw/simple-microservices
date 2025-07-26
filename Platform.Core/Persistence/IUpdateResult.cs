namespace Platform.Core.Persistence;

public interface IUpdateResult<TDto> where TDto : IEntityDto
{
    bool Ok { get; }
    
    TDto? UpdatedEntity { get; set; }
    
    bool VersionConflict { get; set; }
}