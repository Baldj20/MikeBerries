namespace ProductService.BLL.DTO;

public class UpdateProductDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public List<UploadProductImageDto> Images { get; set; } = new();
}
