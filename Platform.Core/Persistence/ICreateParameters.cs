namespace Platform.Core.Persistence;

public interface ICreateParameters<TDto> where TDto : IEntityDto
{
    TDto Entity { get; set; }
}