using System.Data;
using Platform.Domain.EF.Transactions;

namespace Platform.Domain.EF;

public interface IDbScopeProvider : IAsyncDisposable
{
    OperationReaderScope GetReaderScope( 
        bool enableGlobalLock = true,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default);

    OperationModificationScope GetModificationScope( 
        bool enableGlobalLock = true,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default);
}