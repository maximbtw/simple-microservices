namespace Platform.Core.Persistence;

public class EntityUpdateParameters<TDto> : IUpdateParameters<TDto> where TDto : IEntityDto, new()
{
    public TDto Entity { get; set; } = new();
}