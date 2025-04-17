
using Serilog.Context;

namespace CleanArchitecture.Api.Middleware;

public class RequestContextLoggingMiddleware : IMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            return next(context);   
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);
        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
