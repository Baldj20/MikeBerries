using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class MinioFileRepository(MinioStorage storage) : IFileRepository
{
    public async Task DeleteFileAsync(string key, CancellationToken token)
    {
        await storage.DeleteFileAsync(key, token);
    }

    public async Task<Stream> GetFileAsync(string key, CancellationToken token)
    {
        return await storage.GetFileAsync(key, token);
    }

    public async Task<string> UploadFileAsync(string key, Stream fileStream, 
        CancellationToken token)
    {
        return await storage.UploadFileAsync(key, fileStream, token);
    }
}
