using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Roles;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Cliente = new(1, nameof(Cliente));
    public static readonly Role Admin = new(2, nameof(Admin));
    public Role(int id, string name) : base(id, name) { }

    public ICollection<Permission>? Permissions { get; set; }
}
