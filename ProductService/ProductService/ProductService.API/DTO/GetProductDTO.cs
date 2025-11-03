namespace ProductService.BLL.DTO;

public class GetProductDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public List<GetProductImageDto> Images { get; set; } = new();
    public required ProviderDto Provider { get; set; }
}
