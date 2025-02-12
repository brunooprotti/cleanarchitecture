namespace CleanArchitecture.Domain.Vehiculos;

public record VehiculoId(Guid id)
{
    public static VehiculoId New() => new(Guid.NewGuid());
}