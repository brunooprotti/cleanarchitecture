using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions.Specification;

public interface ISpecification<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    //Distintas expresiones que vamoas a aceptar
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Expression<Func<TEntity, object>>>? Includes { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    //Indica cuanto tenemos que tomar
    int Take { get; }
    //Indica cuanto tenemos que saltar
    int Skip { get; }

    //Indica si tenemos que paginar
    bool IsPagingEnabled { get; }
}
