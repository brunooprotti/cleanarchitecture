using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Users;
using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JwtProvider(IOptions<JwtOptions> options, ISqlConnectionFactory sqlConnectionFactory)
    {
        _options = options.Value!;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<string> Generate(User user)
    {
        
        var permissions = await GetUserPermissionsAsync(user.Id!.Value);

        var claims = GetUserClaims(user, permissions);

        var sigingCreadentials = GetTokenSignInCredentials();

        var token = new JwtSecurityToken(
            _options.Issuer, 
            _options.Audience, 
            claims,
            null, 
            DateTime.UtcNow.AddDays(365),
            sigingCreadentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    private async Task<HashSet<string>> GetUserPermissionsAsync(Guid userId)
    {
        const string sql = """
    
                                SELECT
                                    p.nombre
                                FROM users usr
                                    LEFT JOIN users_roles usrl
                                        ON usr.id=usrl.user_id
                                    LEFT JOIN "Roles" rl
                                        ON rl.id=usrl.role_id
                                    LEFT JOIN roles_permissions rlp
                                        ON rl.id=rlp.role_id
                                    LEFT JOIN permissions p
                                        ON p.id=rlp.permission_id
                                    WHERE usr.id=@UserId
                           """;

        using var connection = _sqlConnectionFactory.CreateConnection();
        var permissions = await connection.QueryAsync<string>(sql, new { UserId = userId });

        return permissions.ToHashSet();
    }

    private static List<Claim> GetUserClaims(User user, HashSet<string> permissions)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.ToString())
        };

        foreach (var permission in permissions)
        {
            claims.Add(new (CustomClaims.PERMISSIONS, permission));
        }

        return claims;
    }
        

    private  SigningCredentials GetTokenSignInCredentials()
        => new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
            SecurityAlgorithms.HmacSha256
        );


}