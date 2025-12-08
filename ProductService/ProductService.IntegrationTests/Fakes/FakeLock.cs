using Medallion.Threading;

namespace ProductService.IntegrationTests.Fakes;

public class FakeLock : IDistributedLock
{
    public string Name { get; }

    public FakeLock(string name)
    {
        Name = name;
    }

    public ValueTask<IDistributedSynchronizationHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default) => 
        ValueTask.FromResult<IDistributedSynchronizationHandle>(new FakeHandle());
    public ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default) => 
        ValueTask.FromResult<IDistributedSynchronizationHandle?>(new FakeHandle());
    public IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default) => 
        new FakeHandle();
    public IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default) => 
        new FakeHandle();
}
