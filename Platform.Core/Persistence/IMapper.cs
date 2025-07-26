using Platform.Domain.EF;

namespace Platform.Core.Persistence;

public interface IMapper<in TOrm, TDto> where TOrm : IOrm where TDto : IEntityDto
{
    TDto MapToDto(TOrm orm);

    void UpdateOrmFromDto(TOrm orm, TDto dto);
}