using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Abstractions.Specification;
using CleanArchitecture.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

internal abstract class Repository<TEntity,TEntityId> 
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    //Metodo para llamar a la BDD y desde donde enviamos la Query armada en ApplySpecification
    //Nos va a devolver la lista de records que tenemos
    public async Task<IReadOnlyList<TEntity>> GetAllWithSpec(ISpecification<TEntity, TEntityId> spec)
        => await ApplySpecification(spec).ToListAsync();
    
    //Devuelve la cantidad de records que tenemos.
    public async Task<int> CountAsync(ISpecification<TEntity, TEntityId> spec)
        => await ApplySpecification(spec).CountAsync();


    //Metodo al que llamamos a la implementacion de SpecificationEvaluator para aplicar las condiciones logicas al query inicial.
    public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TEntityId> spec)
    {
        return SpecificationEvaluator<TEntity, TEntityId>
                 .GetQuery(DbContext.Set<TEntity>().AsQueryable(), spec);
    }
}