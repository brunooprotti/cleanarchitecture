using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;