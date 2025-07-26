using Media.Contracts.Persistence.Image;
using Platform.Core.Persistence;
using Platform.Domain.EF.Transactions;

namespace Media.Application.Persistence.Image;

public interface IImageRepository
{
    Task<ImageDto?> GetOrNull(int id, OperationScopeBase scope);

    Task<EntityCreateResult<ImageDto>> Create(
        EntityCreateParameters<ImageDto> parameters,
        OperationModificationScope scope);

    Task<EntityUpdateResult<ImageDto>> Update(
        EntityUpdateParameters<ImageDto> parameters,
        OperationModificationScope scope);

    Task<EntityDeleteResult> Delete(EntityDeleteParameters parameters, OperationModificationScope scope);
}