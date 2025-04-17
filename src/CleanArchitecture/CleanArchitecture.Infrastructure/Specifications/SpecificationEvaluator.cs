using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Abstractions.Specification;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Specifications;

//Esta clase se encarga de evaluar las especificaciones y aplicarlas a la consulta, lo creamos aca porque vamos a acceder a EF Core.
public class SpecificationEvaluator<TEntity,TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{

    //Metodo que devuelve un tipo querable de la entity que busquemos, este objeto una vez que tengamos todo lo que necesitamos, mandamos la query a la DB.
    //De otra forma si pedimos todos los registros y despues filtramos es un uso excesivo de memoria.
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TEntityId> specification)
    {
        //Basicamente usamos el inputQuery y le aplicamos las especificaciones que nos pasan. Es como un patron Builder, que al final ejecuta la query.

        //1. Aplicar los criterios de la especificacion
        if (specification.Criteria is not null)
            inputQuery.Where(specification.Criteria);

        //2. Aplicar el orden de la especificacion
        
        if (specification.OrderBy is not null)
            inputQuery.OrderBy(specification.OrderBy);
        

        if (specification.OrderByDescending is not null)
            inputQuery.OrderByDescending(specification.OrderByDescending);
        

        //3. Aplicar la paginacion de la especificacion
        if (specification.IsPagingEnabled)
            inputQuery.Skip(specification.Skip).Take(specification.Take);

        //4. Aplicar los includes de la especificacion
        //Esto es para que no se carguen todos los registros de la DB, sino que solo los que necesitamos.
        //El AsSplitQuery hace que se divida la query si es muy pesada.
        //El AsNoTracking es para que no se carguen todos los registros de la DB en memoria.
        //El Aggregate funciona como un forEach, pero en este caso lo usamos para aplicar las functions de los includes a la query. 
        //El include no es mas que un Add.

        if (specification.Includes is not null)
            inputQuery = specification.Includes
                                        .Aggregate(
                                            inputQuery,
                                            (current, include) => current.Include(include)
                                        ).AsSplitQuery().AsNoTracking();
        return inputQuery;
    }

}
