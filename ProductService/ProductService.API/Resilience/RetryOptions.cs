namespace ProductService.API.Resilience;

public class RetryOptions
{
    public int MaxRetryAttempts { get; set; } = 3;
    public int DelayMilliseconds { get; set; } = 500;
}
