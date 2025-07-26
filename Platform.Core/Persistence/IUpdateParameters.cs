namespace Platform.Core.Persistence;

public interface IUpdateParameters<TDto> where TDto : IEntityDto
{
    TDto Entity { get; set; }
}