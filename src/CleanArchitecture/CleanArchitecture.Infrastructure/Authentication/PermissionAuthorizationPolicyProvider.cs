using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Authentication;

public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
            return policy;

        // Create a new policy with the permission requirement
        var permissionRequirement = new PermissionRequirement(policyName);

        return new AuthorizationPolicyBuilder()
                    .AddRequirements(permissionRequirement)
                    .Build();
    }
}
