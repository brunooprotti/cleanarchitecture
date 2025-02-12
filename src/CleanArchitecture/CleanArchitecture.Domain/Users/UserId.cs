namespace CleanArchitecture.Domain.Users;

public record UserId(Guid value)
{
    //metodo creador del strongly ID 
    public static UserId New() => new(Guid.NewGuid());
}