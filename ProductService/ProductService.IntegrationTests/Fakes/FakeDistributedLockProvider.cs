using Medallion.Threading;

namespace ProductService.IntegrationTests.Fakes;

public class FakeDistributedLockProvider : IDistributedLockProvider
{
    public IDistributedLock CreateLock(string name)
    {
        return new FakeLock(name);
    }
}
