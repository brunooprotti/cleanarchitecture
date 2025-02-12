using CleanArchitecture.Application.Alquileres.GetAlquiler;
using CleanArchitecture.Application.Alquileres.ReservarAlquiler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Alquileres;

[ApiController]
[Route("api/alquileres")]
public class AlquileresController : ControllerBase
{
    private readonly ISender _sender;

    public AlquileresController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlquiler(
        Guid id,
        CancellationToken cancellationToken
        )
    {
        var query = new GetAlquilerQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReservarAlquiler(
        Guid id,
        AlquilerReservaRequest request,
        CancellationToken cancellationToken
        )
    {
        var query = new ReservarAlquilerCommand(request.VehiculoId, request.UserId, request.StartDate, request.EndDate);

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetAlquiler), new {id = result.Value}, result.Value);
    }
}