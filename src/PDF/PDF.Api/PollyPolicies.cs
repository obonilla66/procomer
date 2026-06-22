/*
 * Políticas de resiliencia para llamadas HTTP externas.
 *
 * Aunque este microservicio actualmente consulta la base directamente,
 * se deja configurado HttpClient con Polly porque el diseño del sistema
 * contempla comunicación entre microservicios.
 *
 * Incluye:
 * - Retry con backoff exponencial.
 * - Circuit breaker.
 * - Timeout.
 */

using Polly;
using Polly.Extensions.Http;

namespace PDF.Api;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }

    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
    }
}
