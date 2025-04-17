using CleanArchitecture.Application.Vehiculos.GetVehiculosByPagination;
using CleanArchitecture.Application.Vehiculos.SearchVehiculos;
using CleanArchitecture.Domain.Abstractions.Specification.Models;
using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.Api.Controllers.Vehiculos;

[ApiController]
[Route("api/vehiculos")]
public class VehiculosController : ControllerBase
{
    private readonly ISender _sender;

    public VehiculosController(ISender sender)
    {
        _sender = sender;
    }

    [HasPermission(PermissionEnum.ReadUser)]
    [HttpGet("search")]
    public async Task<IActionResult> SearchVehiculos( 
        DateOnly startDate, 
        DateOnly endDate, 
        CancellationToken cancellationToken
        )
    {
        var query =  new SearchVehiculosQuery(startDate, endDate);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpGet("getPagination", Name = "PaginationVehiculos")]
    [ProducesResponseType(typeof(PaginationResult<Vehiculo,VehiculoId>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPaginationVehiculo(
        [FromQuery] GetVehiculosByPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }   
}