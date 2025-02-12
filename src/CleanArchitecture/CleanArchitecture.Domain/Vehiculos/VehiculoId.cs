namespace CleanArchitecture.Domain.Vehiculos;

public record VehiculoId(Guid value)
{
    public static VehiculoId New() => new(Guid.NewGuid());
}