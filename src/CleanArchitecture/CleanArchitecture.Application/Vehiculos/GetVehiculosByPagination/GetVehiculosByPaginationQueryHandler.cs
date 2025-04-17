using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Abstractions.Specification.Models;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Vehiculos.Specification;

namespace CleanArchitecture.Application.Vehiculos.GetVehiculosByPagination;

internal sealed class GetVehiculosByPaginationQueryHandler : IQueryHandler<GetVehiculosByPaginationQuery, PaginationResult<Vehiculo, VehiculoId>>
{
    private readonly IVehiculoRepository _vehiculoRepository;

    public GetVehiculosByPaginationQueryHandler(IVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<Result<PaginationResult<Vehiculo, VehiculoId>>> Handle(
        GetVehiculosByPaginationQuery request, 
        CancellationToken cancellationToken
        )
    {
        var spec = new VehiculoPaginationSpecification(request.Sort!, request.PageIndex, request.PageSize, request.Filter!);

        var records = await _vehiculoRepository.GetAllWithSpec(spec);
        var totalRecords = await _vehiculoRepository.CountAsync(spec);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var recordsByPage = records.Count();

        return new PaginationResult<Vehiculo, VehiculoId>
        {
            Count = totalRecords,
            Data = records.ToList(),
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = recordsByPage
        };
    }
}
