namespace Platform.Core.Persistence;

public class EntityUpdateResult<TDto> : IUpdateResult<TDto> where TDto : IEntityDto
{
    public virtual bool Ok => this.UpdatedEntity != null && !this.VersionConflict;

    public TDto? UpdatedEntity { get; set; }
    
    public bool VersionConflict { get; set; }
}