using Medallion.Threading;

namespace ProductService.UnitTests;

public class FakeHandle : IDistributedSynchronizationHandle
{
    public CancellationToken HandleLostToken => CancellationToken.None;

    public void Dispose() 
    {
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
