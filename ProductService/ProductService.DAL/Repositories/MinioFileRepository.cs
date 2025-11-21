using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class MinioFileRepository(MinioStorage storage) : IFileRepository
{
    public async Task DeleteFileAsync(string key)
    {
        await storage.DeleteFileAsync(key);
    }

    public async Task<Stream> GetFileAsync(string key)
    {
        return await storage.GetFileAsync(key);
    }

    public async Task<string> UploadFileAsync(string key, Stream fileStream)
    {
        return await storage.UploadFileAsync(key, fileStream);
    }
}
