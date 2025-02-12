using CleanArchitecture.Application.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Middleware;

public class ExceptionHandlingMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    //Este se ejecuta cuando la REQUEST tiene algun problema o exception dentro de su ejecucion, validaciones, etc.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio una exception: {Message}", ex.Message);
            
            var exceptionDetails = GetExceptionDetails(ex);
            var problemDetails = new ProblemDetails {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail
            };

            if (exceptionDetails.Detail is not null) 
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            
            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception ex)
        =>  ex switch 
            {
                ValidationException validationException => new ExceptionDetails(
                                                                StatusCodes.Status400BadRequest, 
                                                                "Validation Failure", 
                                                                "Validacion de error", 
                                                                "han ocurrido uno o mas errores de validacion", 
                                                                validationException.Errors),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "Server error",
                    "Error de servidor",
                    "Un inesperado error a ocurrido en la app",
                    null
                )
            };
    

    internal record ExceptionDetails
    (
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors
    );
}