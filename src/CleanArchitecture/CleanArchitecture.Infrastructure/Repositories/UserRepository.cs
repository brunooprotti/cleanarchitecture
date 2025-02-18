using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

//Si declaramos una clase como UserRepository donde tenemos una implementacion Generica, podemos usar una interfaz para extender estos metodos./
//Entonces todos los metodos nuevos van a ir para IUserRepository y la implementacion dentro de UserRepository porque son especificos para esa clase.
internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext){ }

    public async Task<User?> GetByEmailAsync(Domain.Users.Email email, CancellationToken cancellationToken = default)
        => await DbContext.Set<User>()
                          .FirstOrDefaultAsync(x=> x.Email == email, cancellationToken);
    
    public async Task<bool> IsUserExists(Domain.Users.Email email, CancellationToken cancellationToken = default)
        => await DbContext.Set<User>().AnyAsync(u => u.Email == email, cancellationToken);
}