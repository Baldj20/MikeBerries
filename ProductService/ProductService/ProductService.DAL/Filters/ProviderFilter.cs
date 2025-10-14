using ProductService.DAL.Builders;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Filters;

namespace ProductService.DAL.Filters;

public class ProviderFilter : IFilter<Provider>
{
    public string? Name { get; set; }
    public string? Product { get; set; }

    public IQueryable<Provider> Apply(IQueryable<Provider> query)
    {
        return query.WithName(Name)
                    .WithProduct(Product);
    }
}
