using Media.Contracts.Persistence.Image;
using Media.Domain.Image;
using Platform.Core.Persistence;

namespace Media.Application.Persistence.Image;

internal class ImageRepository : EntityRepositoryBase<
        ImageOrm,
        ImageDto,
        EntityCreateParameters<ImageDto>,
        EntityCreateResult<ImageDto>,
        EntityUpdateParameters<ImageDto>,
        EntityUpdateResult<ImageDto>,
        EntityDeleteParameters,
        EntityDeleteResult>,
    IImageRepository
{
    public override ImageMapper GetMapper() => new();
}