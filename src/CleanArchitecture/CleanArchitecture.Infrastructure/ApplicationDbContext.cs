using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    public ApplicationDbContext(DbContextOptions options, IPublisher publisher): base(options) 
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Esta linea lo que hace es buscar a todos los IEntityTypeConfiguration que declaramos (los configurations) y los aplica en la creacion del modelo.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await SaveChangesAsync(cancellationToken);

            await PublishDomainEventAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex) //Se lanza cuando ocurre cualquier tipo de error al ingresar un registro en la BDD
        {
            throw new ConcurrencyException("La excepcion por concurrencia se disparo", ex);
        }
        
    }

    private async Task PublishDomainEventAsync()
    {
        var domainEvents = ChangeTracker
                            .Entries<Entity>()
                            .Select(entry => entry.Entity)
                            .SelectMany(entity => {
                                var domainEvents = entity.GetDomainEvents();
                                entity.ClearDomainEvents();
                                return domainEvents;
                            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }

    }
}