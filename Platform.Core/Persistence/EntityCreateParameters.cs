namespace Platform.Core.Persistence;

public class EntityCreateParameters<TDto> : ICreateParameters<TDto> where TDto : IEntityDto, new()
{
    public TDto Entity { get; set; } = new();
}