using CleanArchitecture.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Abstractions.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken
        )
    {
        //Necesitamos saber que command se esta evaluando, si nos fijamos en la firma de la clase, solo declaramos para los Commands. No soporta queries.

        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation($"Ejecutando el command request: {name}");
            var result = await next();
            _logger.LogInformation($"El comando {name} se ejecuto correctamente");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"El comando {name} tuvo errores");
            throw;
        }
    }
}
