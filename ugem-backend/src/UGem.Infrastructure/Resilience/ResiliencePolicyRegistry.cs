using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;

namespace UGem.Infrastructure.Resilience;

public static class ResiliencePolicyRegistry
{
    // Prevent retry storm with Exponential Backoff + Jitter
    public static AsyncRetryPolicy CreateRetryPolicy(int retryCount = 3)
    {
        var random = new Random();
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) 
                                + TimeSpan.FromMilliseconds(random.Next(0, 100)),
                (exception, timeSpan, retry, context) =>
                {
                    // Log retry attempt with context
                });
    }

    public static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy()
    {
        return Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (ex, breakDelay) => { /* Notify SRE */ },
                onReset: () => { /* Log reset */ }
            );
    }
}
