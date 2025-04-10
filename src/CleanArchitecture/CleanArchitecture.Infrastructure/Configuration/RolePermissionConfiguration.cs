using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configuration;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("roles_permissions");
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.Property(x => x.PermissionId)
            .HasConversion(
                permissionId => permissionId!.Value, 
                value => new Domain.Permissions.PermissionId(value)
            );

        builder.HasData(
                Create(Role.Cliente, PermissionEnum.ReadUser),
                Create(Role.Admin, PermissionEnum.ReadUser),
                Create(Role.Admin, PermissionEnum.WriteUser),
                Create(Role.Admin, PermissionEnum.UpdateUser)
            );
    }

    private static RolePermission Create(Role role, PermissionEnum permission)
        => new RolePermission
        {
            RoleId = role.Id,
            PermissionId = new PermissionId((int)permission)
        };
}
