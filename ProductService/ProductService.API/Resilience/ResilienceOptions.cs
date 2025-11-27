namespace ProductService.API.Resilience;

public class ResilienceOptions
{
    public const string CONFIG_SECTION_NAME = "ResilienceSettings";
    public RetryOptions Retry { get; set; } = new();
    public CircuitBreakerOptions CircuitBreaker { get; set; } = new();
}
