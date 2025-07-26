using Microsoft.Extensions.Logging;

namespace Platform.Domain.EF;

public interface IDbEventObserver
{
    void HandleSql(LogLevel logLevel, TimeSpan duration, string sql);

    void HandleEndOperationModificationScope(DateTime beginDateTime);
    
    void HandleEndOperationReaderScope(DateTime beginDateTime);
    
    void HandleUnexpectedException(Exception exception);
}