using ProductService.DAL.Interfaces.Repositories;
using System.Text;

namespace ProductService.IntegrationTests;

public class FakeFileRepository : IFileRepository
{
    public Task DeleteFileAsync(string key)
    {
        return Task.CompletedTask;
    }

    public Task<Stream> GetFileAsync(string key)
    {
        var fakeContent = "Fake file content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fakeContent));
        return Task.FromResult<Stream>(stream);
    }

    public Task<string> UploadFileAsync(string key, Stream fileStream)
    {
        var fakeUrl = $"http://fake-minio-host/bucket/{key}";
        return Task.FromResult(fakeUrl);
    }
}
