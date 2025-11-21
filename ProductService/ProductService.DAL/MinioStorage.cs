using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace ProductService.DAL;

public class MinioStorage
{
    private readonly AmazonS3Client _client;
    private readonly string _bucketName;
    private readonly string _serviceUrl;

    public MinioStorage(MinioSettings settings)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = settings.ServiceURL,
            ForcePathStyle = true
        };

        var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
        _client = new AmazonS3Client(credentials, config);
        _bucketName = settings.BucketName;
        _serviceUrl = settings.ServiceURL;
    }

    public async Task EnsureBucketExistsAsync()
    {
        bool exists = await BucketExistsAsync(_bucketName);
        if (!exists)
        {
            await _client.PutBucketAsync(new PutBucketRequest { BucketName = _bucketName });
        }
    }

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        try
        {
            await _client.GetBucketLocationAsync(bucketName);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
                return false;
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string key, Stream fileStream)
    {
        await EnsureBucketExistsAsync();

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream
        };

        await _client.PutObjectAsync(request);

        var uriBuilder = new UriBuilder(_serviceUrl)
        {
            Path = Path.Combine(_bucketName, key).Replace('\\', '/')
        };

        return uriBuilder.ToString();
    }

    public async Task<Stream> GetFileAsync(string key)
    {
        var response = await _client.GetObjectAsync(_bucketName, key);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _client.DeleteObjectAsync(request);
    }
}

