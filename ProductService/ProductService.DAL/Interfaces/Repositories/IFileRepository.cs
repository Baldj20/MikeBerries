namespace ProductService.DAL.Interfaces.Repositories;

public interface IFileRepository
{
    Task<string> UploadFileAsync(string key, Stream fileStream, 
        CancellationToken token);
    Task<Stream> GetFileAsync(string key, CancellationToken token);
    Task DeleteFileAsync(string key, CancellationToken token);
}
