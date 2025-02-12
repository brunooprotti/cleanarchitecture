using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public sealed record Rating
{
    public static readonly Error Invalid = new("Rating.Invalid", "El rating es invalido");

    public int Value { get; init; }
    
    private Rating(int value) => Value = value;

    public static Result<Rating> Create(int value) => value < 1 || value > 5
                                                      ? Result.Failure<Rating>(Invalid)
                                                      : new Rating(value);             
}
