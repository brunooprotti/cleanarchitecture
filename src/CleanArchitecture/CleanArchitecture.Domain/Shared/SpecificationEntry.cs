namespace CleanArchitecture.Domain.Shared;

public record SpecificationEntry
{
    private const int MAX_PAGE_SIZE = 50;
    private const int DEFAULT_PAGE_SIZE = 10;
    public string? Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    
    //Aca validamos para que no podamos mandarle 10000 registros de una
    public int PageSize {
        get => DEFAULT_PAGE_SIZE;
        set => PageSize = ValidatePageSize(value);
    }

    public string? Filter { get; set; }

    private int ValidatePageSize(int value)
        => value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
}
