using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<string> Generate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.ToString())
        };

        var sigingCreadentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            _options.Issuer, 
            _options.Audience, 
            claims,
            null, 
            DateTime.UtcNow.AddDays(365),
            sigingCreadentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult<string>(tokenValue);
    }
}