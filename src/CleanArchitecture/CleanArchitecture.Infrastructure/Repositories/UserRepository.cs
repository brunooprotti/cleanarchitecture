using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Infrastructure.Repositories;

//Si declaramos una clase como UserRepository donde tenemos una implementacion Generica, podemos usar una interfaz para extender estos metodos./
//Entonces todos los metodos nuevos van a ir para IUserRepository y la implementacion dentro de UserRepository porque son especificos para esa clase.
internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext){ }

}