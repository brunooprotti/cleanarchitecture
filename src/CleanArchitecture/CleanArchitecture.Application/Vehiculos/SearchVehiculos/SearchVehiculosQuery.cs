using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos;

/// <summary>
/// Define el objeto para buscar vehiculos disponibles para el alquiler.
/// </summary>
public sealed record SearchVehiculosQuery (
    DateOnly fechaInicio, 
    DateOnly fechaFin
) : IQuery<IReadOnlyList<VehiculoResponse>>;