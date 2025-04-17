using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Authentication;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        
        //1 - existe un user dentro el token?
        string? userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Task.CompletedTask;

        //2 - Validacion de roles dentro del token
        HashSet<string> permissions = context.User.Claims
                                       .Where(c => c.Type == CustomClaims.PERMISSIONS)
                                       .Select(x => x.Value).ToHashSet();

        //3 - Si tiene el permiso requerido agregamos la autorizacion en el contexto
        if(permissions.Contains(requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
