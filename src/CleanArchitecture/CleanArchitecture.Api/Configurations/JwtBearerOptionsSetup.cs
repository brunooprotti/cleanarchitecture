using System.Text;
using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Api.Configurations;


/// <summary>
/// Clase donde tomamos la info del appSettings y la seteamos dentro del JwtBearerOptions para compararlo contra el que envia el usuario.
/// </summary>
public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    /// <summary>
    /// JwtOptions que contiene la info del appSettings
    /// </summary>
    private readonly JwtOptions _jwtOptions; 

    public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public void Configure(string? name, JwtBearerOptions options) => ConfigureToken(options);
    

    public void Configure(JwtBearerOptions options) => ConfigureToken(options);
    

    private void ConfigureToken(JwtBearerOptions options)
        => options.TokenValidationParameters = new () 
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = GetSymmetricSecurityKey()
        };

    private SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!));
    
}