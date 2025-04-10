using CleanArchitecture.Application.Users;
using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configuration;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("users_roles");
        
        builder.HasKey(userRole => new { userRole.RoleId, userRole.UserId });
        
        builder.Property(userRole => userRole.RoleId);
        
        builder.Property(userRole => userRole.UserId)
               .HasConversion(userId => userId!.Value, value => new UserId(value));
    }
}
