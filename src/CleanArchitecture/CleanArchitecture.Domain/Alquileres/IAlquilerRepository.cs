using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public interface IAlquilerRepository
{
    Task<Alquiler?> GetByIdAsync(AlquilerId AlquilerId, CancellationToken cancellationToken = default);
    Task<bool> IsOverlappingAsync(Vehiculo vehiculo, DateRange duracion, CancellationToken cancellationToken = default);
    void Add(Alquiler alquiler);
}
