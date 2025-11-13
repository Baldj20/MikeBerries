namespace ProductService.DAL;

public record PaginationParams
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public void Deconstruct(out int page, out int pageSize)
    {
        page = Page;
        pageSize = PageSize;
    }
}
