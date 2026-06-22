/*
 * Políticas de resiliencia para llamadas HTTP externas.
 *
 * El microservicio Agenda puede requerir comunicación con otros servicios.
 * Por eso se dejan preparadas políticas de:
 * - Reintentos con backoff exponencial.
 * - Circuit breaker.
 * - Timeout.
 *
 * Estas políticas evitan que una falla externa bloquee indefinidamente
 * el proceso de scheduling.
 */

using Polly;
using Polly.Extensions.Http;

namespace Agenda.Api;

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
