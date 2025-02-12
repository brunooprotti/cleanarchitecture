using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(Guid AlquilerId) : IDomainEvent;