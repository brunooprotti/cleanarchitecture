using CleanArchitecture.Application.Users;
using CleanArchitecture.Application.Users.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Users;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Login([FromBody] LoginUserRequest request , CancellationToken cancellationToken) 
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return Unauthorized(result.Error);

        return Ok(result.Value);
    }
}