using CleanArchitecture.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configuration;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasConversion( permissionId => permissionId!.Value, value => new PermissionId(value));

        builder.Property(x => x.Nombre)
               .HasConversion(nombre => nombre!.Value, value => new Nombre(value));

        IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
            .Select(p => 
                new Permission( 
                    new PermissionId((int)p), 
                    new Nombre(p.ToString()) 
                ) 
            );

        builder.HasData(permissions);
    }
}
