using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class MinioFileRepository(MinioStorage storage) : IFileRepository
{
    private MinioStorage _storage { get; } = storage;

    public async Task DeleteFileAsync(string key)
    {
        await _storage.DeleteFileAsync(key);
    }

    public async Task<Stream> GetFileAsync(string key)
    {
        return await _storage.GetFileAsync(key);
    }

    public async Task<string> UploadFileAsync(string key, Stream fileStream)
    {
        return await _storage.UploadFileAsync(key, fileStream);
    }
}
