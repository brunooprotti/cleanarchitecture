namespace CleanArchitecture.Domain.Reviews;

public record ReviewId(Guid value)
{
    public ReviewId New() => new ReviewId(Guid.NewGuid());
}