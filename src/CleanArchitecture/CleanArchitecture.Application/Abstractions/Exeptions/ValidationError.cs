namespace CleanArchitecture.Application.Abstractions.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage);