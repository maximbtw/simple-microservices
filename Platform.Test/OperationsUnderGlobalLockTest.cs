using System.Collections.Concurrent;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Xunit.Abstractions;

namespace Platform.Test;

public class OperationsUnderGlobalLockTest
{
    private readonly ITestOutputHelper _output;

    public OperationsUnderGlobalLockTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task ReadWait_WhenWriteLockFirstIsHeld()
    {
        await using var dbExecutor = new TestDbExecutorProvider(new ReaderWriterLockProvider());
        
        Task task1 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider =  dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Writer scope ready to create");
            await using (OperationModificationScope scope = scopeProvider.GetModificationScope())
            {
                _output.WriteLine("Writer scope created");
                await scope.SaveAsync();
                await Task.Delay(10000); 
                await scope.CommitChangesIfSucceededAsync(true);    
            }
            
            _output.WriteLine("Writer scope disposed");
        });

        await Task.Delay(100); 

        Task task2 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider = dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Reader scope ready to create");
            await using (OperationReaderScope scope = scopeProvider.GetReaderScope())
            {
                _output.WriteLine("Reader scope created");
                scope.GetDbContext();
                await Task.Delay(1000);    
            }
            
            _output.WriteLine("Reader scope disposed");
        });
        
        Task completed = await Task.WhenAny(task1, task2);
        if (completed == task2)
        {
            Assert.False(true);
        }

        await Task.WhenAll(task1, task2); 
    }
    
    [Fact]
    public async Task WriteWait_WhenWriteLockFirstIsHeld()
    {
        await using var dbExecutor = new TestDbExecutorProvider(new ReaderWriterLockProvider());
        
        Task task1 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider =  dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Writer1 scope ready to create");
            await using (OperationModificationScope scope = scopeProvider.GetModificationScope())
            {
                _output.WriteLine("Writer1 scope created");
                await scope.SaveAsync();
                await Task.Delay(500); 
                await scope.CommitChangesIfSucceededAsync(true);    
            }
            
            _output.WriteLine("Writer1 scope disposed");
        });

        await Task.Delay(100); 

        Task task2 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider = dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Writer2 scope ready to create");
            await using (OperationModificationScope scope = scopeProvider.GetModificationScope())
            {
                _output.WriteLine("Writer2 scope created");
                await scope.SaveAsync();
                await Task.Delay(200); 
                await scope.CommitChangesIfSucceededAsync(true);     
            }
            
            _output.WriteLine("Writer2 scope disposed");
        });
        
        Task completed = await Task.WhenAny(task1, task2);
        if (completed == task2)
        {
            Assert.False(true);
        }

        await Task.WhenAll(task1, task2); 
    }
    
    [Fact]
    public async Task WriteWait_WhenReadLockFirstIsHeld()
    {
        await using var dbExecutor = new TestDbExecutorProvider(new ReaderWriterLockProvider());
        
        Task task1 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider = dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Read scope ready to create");
            await using (OperationReaderScope scope = scopeProvider.GetReaderScope())
            {
                _output.WriteLine("Read scope created");
                scope.GetDbContext();
                await Task.Delay(500); 
            }
            
            _output.WriteLine("Read scope disposed");
        });

        await Task.Delay(100); 

        Task task2 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider = dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Writer scope ready to create");
            await using (OperationModificationScope scope = scopeProvider.GetModificationScope())
            {
                _output.WriteLine("Writer scope created");
                await scope.SaveAsync();
                await Task.Delay(200); 
                await scope.CommitChangesIfSucceededAsync(true);    
            }
            
            _output.WriteLine("Writer scope disposed");
        });
        
        Task completed = await Task.WhenAny(task1, task2);
        if (completed == task2)
        {
            Assert.False(true);
        }

        await Task.WhenAll(task1, task2); 
    }
    
    [Fact]
    public async Task ReadParallel_WhenReadLockFirstIsHeld()
    {
        await using var dbExecutor = new TestDbExecutorProvider(new ReaderWriterLockProvider());

        var tcs1 = new TaskCompletionSource();
        var tcs2 = new TaskCompletionSource();
        
        Task task1 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider =  dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Read1 scope ready to create");
            await using (OperationReaderScope scope = scopeProvider.GetReaderScope())
            {
                _output.WriteLine("Read1 scope created");
                tcs1.SetResult();
                scope.GetDbContext();
                await Task.Delay(400); 
            }
            
            _output.WriteLine("Read1 scope disposed");
        });
        
        Task task2 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider = dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Read2 scope ready to create");
            await using (OperationReaderScope scope = scopeProvider.GetReaderScope())
            {
                _output.WriteLine("Read2 scope created");
                tcs2.SetResult();
                scope.GetDbContext();
                await Task.Delay(400);     
            }
            
            _output.WriteLine("Read2 scope disposed");
        });

        await Task.WhenAny(task1, task2); 
        
        Assert.Equal(tcs1.Task.IsCompleted, tcs2.Task.IsCompleted);
        
        await Task.WhenAll(task1, task2);
    }
    
    [Fact]
    public async Task ReadLock_WaitsForWriteLock_AndWriteLockWaitsForReadLock()
    {
        await using var dbExecutor = new TestDbExecutorProvider(new ReaderWriterLockProvider());

        var signals = new ConcurrentBag<int>();
        
        Task task1 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider =  dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Writer1 scope ready to create");
            await using (OperationModificationScope scope = scopeProvider.GetModificationScope())
            {
                _output.WriteLine("Writer1 scope created");
                signals.Add(1);
                await scope.SaveAsync();
                await Task.Delay(200); 
                await scope.CommitChangesIfSucceededAsync(true);    
            }
            _output.WriteLine("Writer1 scope disposed");
            
            await Task.Delay(100);
            
            _output.WriteLine("Writer2 scope ready to create");
            await using (OperationModificationScope scope = scopeProvider.GetModificationScope())
            {
                _output.WriteLine("Writer2 scope created");
                signals.Add(3);
                await scope.SaveAsync();
                await Task.Delay(200); 
                await scope.CommitChangesIfSucceededAsync(true);    
            }
            _output.WriteLine("Writer2 scope disposed");
        });
        
        Task task2 = Task.Run(async () =>
        {
            await using IDbScopeProvider scopeProvider = dbExecutor.GetDbScopeProviderAsync();
            _output.WriteLine("Read scope ready to create");
            await using (OperationReaderScope scope = scopeProvider.GetReaderScope())
            {
                _output.WriteLine("Read scope created");
                signals.Add(2);
                scope.GetDbContext();
                await Task.Delay(400);     
            }
            
            _output.WriteLine("Read scope disposed");
        });

        await Task.WhenAll(task1, task2);
        
        Assert.Equal(new[] {1, 2, 3}, signals.Reverse().ToList());
    }
}

