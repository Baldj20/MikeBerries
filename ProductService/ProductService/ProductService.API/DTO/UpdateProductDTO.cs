namespace ProductService.BLL.DTO;

public class UpdateProductDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<UploadProductImageDTO> Images { get; set; }
}
