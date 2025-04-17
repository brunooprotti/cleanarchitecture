using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace CleanArchitecture.Application.Abstractions.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IBaseRequest 
    where TResponse : Result
{
    private readonly ILogger<LoggingBehaviour<TRequest,TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken
        )
    {
        //Necesitamos saber que command o querie se esta evaluando.

        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation($"Ejecutando el request: {name}", name);
            var result = await next();

            if (result.IsSuccess)
                _logger.LogInformation($"El request {name} fue exitoso", name);

            if (result.IsFailure)
                using (LogContext.PushProperty( "Error", result.Error, true))
                {
                    _logger.LogError($"El request {name} tuvo errores", name);
                }

            _logger.LogInformation($"El request {name} se ejecutó correctamente", name);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"El request {name} tuvo errores", name);
            throw;
        }
    }
}
