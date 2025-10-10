namespace ProductService.BLL.DTO;

public class UpdateProductDTO
{
    public Guid ProductId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<UploadProductImageDTO> Images { get; set; }
    public ProviderDTO Provider { get; set; }
    public Guid ProviderId { get; set; }
}
