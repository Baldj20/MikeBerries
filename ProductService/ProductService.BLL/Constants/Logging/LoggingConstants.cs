namespace ProductService.BLL.Constants.Logging;

public static class LoggingConstants
{
    public const string RESOURCE_ADDED = "{ResourceName} with id {ResourceId} added";
    public const string RESOURCE_UPDATED = "{ResourceName} with id {ResourceId} updated";
    public const string RESOURCE_DELETED = "{ResourceName} with id {ResourceId} deleted";
    public const string RESOURCE_RETURNED = "{ResourceName} with id {ResourceId} returned";
    public const string RESOURCE_NOT_FOUND = "{ResourceName} with id {ResourceId} not found";
    public const string RESOURCES_FILTERED_NOT_FOUND = "{ResourceName}s with these filters not found";

    public const string RESOURCE_TO_UPDATE_NOT_FOUND = "{ResourceName} to update not found";
    public const string RESOURCE_TO_DELETE_NOT_FOUND = "{ResourceName} to delete not found";
}
