namespace CleanArchitecture.Domain.Reviews;

public record ReviewId(Guid value)
{
    public static ReviewId New() => new ReviewId(Guid.NewGuid());
}