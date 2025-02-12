using CleanArchitecture.Application.Abstractions.Exceptions;
using CleanArchitecture.Application.Abstractions.Messaging;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CleanArchitecture.Application.Abstractions.Behaviours;

//Pipeline behaviour SOLO para las exceptions generadas por las validaciones
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any()){
            return await next();
        }
        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validators => validators.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(ValidationFailure => new ValidationError(ValidationFailure.PropertyName, ValidationFailure.ErrorMessage)).ToList();

        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next();
    }
}

