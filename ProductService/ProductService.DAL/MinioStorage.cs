using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Polly;
using Polly.Registry;
using System.Net;

namespace ProductService.DAL;

public class MinioStorage
{
    private readonly AmazonS3Client _client;
    private readonly string _bucketName;
    private readonly string _serviceUrl;
    private readonly ResiliencePipeline _pipeline;

    public MinioStorage(MinioSettings settings, 
        ResiliencePipelineProvider<string> pipelineProvider,
        string pipelineName = "standard-pipeline")
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
        _pipeline = pipelineProvider.GetPipeline(pipelineName);
    }

    public async Task EnsureBucketExistsAsync(CancellationToken token = default)
    {
        bool exists = await BucketExistsAsync(_bucketName, token);
        if (!exists)
        {
            await _client.PutBucketAsync(new PutBucketRequest { BucketName = _bucketName });
        }
    }

    public async Task<bool> BucketExistsAsync(string bucketName, CancellationToken token = default)
    {
        try
        {
            await _client.GetBucketLocationAsync(bucketName, token);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound)
                return false;
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string key, Stream fileStream, 
        CancellationToken token)
    {
        await _pipeline.ExecuteAsync(async ct =>
        {
            if (fileStream.CanSeek) fileStream.Position = 0;

            await EnsureBucketExistsAsync();

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream
            };

            await _client.PutObjectAsync(request, ct);
        }, token);

        var uriBuilder = new UriBuilder(_serviceUrl)
        {
            Path = Path.Combine(_bucketName, key).Replace('\\', '/')
        };

        return uriBuilder.ToString();
    }

    public async Task<Stream> GetFileAsync(string key, CancellationToken token)
    {
        var response = await _pipeline.ExecuteAsync(async ct =>
        {
            return await _client.GetObjectAsync(_bucketName, key, ct);
        }, token);

        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string key, CancellationToken token)
    {
        await _pipeline.ExecuteAsync(async ct =>
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _client.DeleteObjectAsync(request, ct);
        }, token);
    }
}

