namespace ProductService.BLL.Models;

public class UpdateProductModel
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public List<UpdateImageModel> Images { get; set; } = new();
}
