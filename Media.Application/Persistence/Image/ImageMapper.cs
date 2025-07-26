using Media.Contracts.Persistence.Image;
using Media.Domain.Image;
using Platform.Core.Persistence;

namespace Media.Application.Persistence.Image;

internal class ImageMapper : IMapper<ImageOrm, ImageDto>
{
    public ImageDto MapToDto(ImageOrm orm) => new()
    {
        Id = orm.Id,
        Version = orm.Version,
        Url = orm.Url,
        PublicId = orm.PublicId,
        Folder = orm.Folder
    };

    public void UpdateOrmFromDto(ImageOrm orm, ImageDto dto)
    {
        orm.Id = dto.Id;
        orm.Version = dto.Version;
        orm.Url = dto.Url;
        orm.PublicId = dto.PublicId;
        orm.Folder = dto.Folder;
    }
}