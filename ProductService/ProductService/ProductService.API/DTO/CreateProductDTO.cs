namespace ProductService.BLL.DTO;

public class CreateProductDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public List<UploadProductImageDto> Images { get; set; } = new();
    public required ProviderDto Provider { get; set; }
}
