namespace CleanArchitecture.Domain.Alquileres;

public record AlquilerId(Guid value)
{
    public static AlquilerId New() => new(Guid.NewGuid());
}