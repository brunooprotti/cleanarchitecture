using System.Text;
using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Api.Configurations;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions> //Clase para leer configs desde el appSettings
{
    private const string SECTION_NAME = "Jwt";
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
        => _configuration.GetSection(SECTION_NAME).Bind(options);//Basicamente leemos las properties en esa Section y las bindeamos a la clase JwtOptions
    
}