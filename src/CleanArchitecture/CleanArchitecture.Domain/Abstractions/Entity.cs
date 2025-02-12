namespace CleanArchitecture.Domain.Abstractions;


//Clase de la cual heredan todas
public abstract class Entity<TEntityId> : IEntity
{
    protected Entity() { }

    //objeto al cual se van a agregar los IDomainEvents
    private readonly List<IDomainEvent> _domainEvents = new ();
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    public TEntityId? Id { get; init; } 

    //obtener todos los IDomainEvents
    public IReadOnlyList<IDomainEvent> GetDomainEvents() 
        => _domainEvents.ToList();

    //Limpiar todos los IDomainEvents
    public void ClearDomainEvents() 
        => _domainEvents.Clear();

    //Agregar un nuevo IDomainEvent el cual solo pueden utilizar los hijos por ser "protected"
    protected void RaiseDomainEvent(IDomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
}
