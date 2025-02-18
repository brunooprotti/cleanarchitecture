namespace CleanArchitecture.Domain.Users;

public record UserId(Guid Value)
{
    //metodo creador del strongly ID 
    public static UserId New() => new(Guid.NewGuid());
}