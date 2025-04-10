using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Users
{
    public sealed class UserRole
    {
        public int RoleId { get; set; }
        public UserId? UserId { get; set; }
    }
}
