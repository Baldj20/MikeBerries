using Microsoft.Extensions.Logging;

namespace ProductService.BLL.Logging;

public static partial class HighPerfomanceLoggers
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information,
    Message = "{ResourceName} with id {ResourceId} added")]
    public static partial void ResourceAdded(
        this ILogger logger,
        string resourceName,
        Guid resourceId
    );

    [LoggerMessage(EventId = 2, Level = LogLevel.Information,
    Message = "{ResourceName} with id {ResourceId} updated")]
    public static partial void ResourceUpdated(
        this ILogger logger,
        string resourceName,
        Guid resourceId
    );

    [LoggerMessage(EventId = 3, Level = LogLevel.Information,
    Message = "{ResourceName} with id {ResourceId} deleted")]
    public static partial void ResourceDeleted(
        this ILogger logger,
        string resourceName,
        Guid resourceId
    );

    [LoggerMessage(EventId = 4, Level = LogLevel.Information,
    Message = "{ResourceName} with id {ResourceId} returned")]
    public static partial void ResourceReturned(
        this ILogger logger,
        string resourceName,
        Guid resourceId
    );

    [LoggerMessage(EventId = 5, Level = LogLevel.Information,
    Message = "{ResourceName} with id {ResourceId} not found")]
    public static partial void ResourceNotFound(
        this ILogger logger,
        string resourceName,
        Guid resourceId
    );

    [LoggerMessage(EventId = 6, Level = LogLevel.Warning,
    Message = "{ResourceName}s with these filters not found")]
    public static partial void FilteredResourcesNotFound(
        this ILogger logger,
        string resourceName
    );

    [LoggerMessage(EventId = 7, Level = LogLevel.Warning,
    Message = "{ResourceName} to update not found")]
    public static partial void ResourceToUpdateNotFound(
        this ILogger logger,
        string resourceName
    );

    [LoggerMessage(EventId = 8, Level = LogLevel.Warning,
    Message = "{ResourceName} to delete not found")]
    public static partial void ResourceToDeleteNotFound(
        this ILogger logger,
        string resourceName
    );
}
