namespace ProductService.DAL;

public class MinioSettings
{
    public const string CONFIG_SECTION_NAME = "Minio";
    public required string ServiceUrl { get; set; }
    public required string DockerContainerUrl { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string BucketName { get; set; }
}
