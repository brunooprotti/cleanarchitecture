namespace CleanArchitecture.Domain.Abstractions.Specification.Models;

public sealed class PaginationResult<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    public int Count { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyList<TEntity>? Data { get; set; }
    public int PageCount { get; set; }
    public int ResultByPage { get; set; }
}
