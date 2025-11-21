namespace ProductService.DAL.Interfaces.Repositories;

public interface IFileRepository
{
    Task<string> UploadFileAsync(string key, Stream fileStream);
    Task<Stream> GetFileAsync(string key);
    Task DeleteFileAsync(string key);
}
