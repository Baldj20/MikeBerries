using Medallion.Threading;

namespace ProductService.IntegrationTests.Fakes;

public class FakeHandle : IDistributedSynchronizationHandle
{
    public CancellationToken HandleLostToken => CancellationToken.None;

    public void Dispose() { }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
