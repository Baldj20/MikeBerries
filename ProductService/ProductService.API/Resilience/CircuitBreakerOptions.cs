namespace ProductService.API.Resilience;

public class CircuitBreakerOptions
{
    public double FailureRatio { get; set; } = 0.5;
    public int SamplingDurationSeconds { get; set; } = 30;
    public int MinimumThroughput { get; set; } = 10;
    public int BreakDurationSeconds { get; set; } = 15;
}
