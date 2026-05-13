namespace Seamstress.VSA.Domain.Shared;

public class PagedResponse<T>
{
    public IReadOnlyList<T> Data { get; init; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;

    public static PagedResponse<T> Create(IReadOnlyList<T> data, int totalCount,
        int page, int size)
    {
        return new PagedResponse<T>()
        {
            Data = data,
            TotalRecords = totalCount,
            PageNumber = page,
            PageSize = size,
            TotalPages = (int)Math.Ceiling(totalCount / (double)size)
        };
    }
}