
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Alquileres.ReservarAlquiler;

public record ReservarAlquilerCommand(Guid VehiculoId, Guid UserId, DateOnly FechaInicio, DateOnly FechaFin) : ICommand<Guid>;