namespace CleanArchitecture.Domain.Abstractions;

public interface IUnitOfWork
{
    //Con el cancelationToken lo que hacemos es abortar una operacion que lleve demasiado tiempo en proceso, se hace para no bloquear el trafico.
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
