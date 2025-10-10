namespace ProductService.BLL.DTO;

public class GetProductDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<ProductImageGetDTO> Images { get; set; }
    public ProviderDTO Provider { get; set; }
}
