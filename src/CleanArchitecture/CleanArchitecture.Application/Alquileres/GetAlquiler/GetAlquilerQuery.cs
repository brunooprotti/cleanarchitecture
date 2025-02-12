using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Alquileres;

namespace CleanArchitecture.Application.Alquileres.GetAlquiler;

public sealed record GetAlquilerQuery(Guid AlquilerId): IQuery<AlquilerResponse>;