using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error) 
    {
        //Este error se lanza porque no puede ser Success y a la vez tener un error
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }  

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) 
        => new(value, true, Error.None);
    
    public static Result<TValue> Failure<TValue>(Error error) 
        => new(default, false, error);

    //Filtra por si es nulo o no.
    public static Result<TValue> Create<TValue>(TValue? value)
        => value is not null
        ? Success(value)
        : Failure<TValue>(Error.NullValue);

}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    
    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess,error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("El resultado del valor de error no es admisible");

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}
