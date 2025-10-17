namespace ProductService.BLL.Constants.Logging;

public static class LoggingConstants<T>
{
    public static readonly string RESOURCE_ADDED = typeof(T).Name + " with id {0} added";
    public static readonly string RESOURCE_UPDATED = typeof(T).Name + " with id {0} updated";
    public static readonly string RESOURCE_DELETED = typeof(T).Name + " with id {0} deleted";
    public static readonly string RESOURCE_RETURNED = typeof(T).Name + " with id {0} returned";
    public static readonly string RESOURCE_NOT_FOUND = typeof(T).Name + "{0} with id {1} not found";
    public static readonly string RESOURCES_FILTERED_NOT_FOUND = typeof(T).Name + "s with these filters not found";
}
