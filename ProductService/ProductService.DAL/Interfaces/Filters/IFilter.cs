namespace ProductService.DAL.Interfaces.Filters;

public interface IFilter<T>
{
    IQueryable<T> Apply(IQueryable<T> query);
}
