namespace Zimmj.Core.CrossCutting.Search;

public class SearchAnswer<TEntity>
{
    public IReadOnlyList<TEntity> Items { get; init; } = Array.Empty<TEntity>();
    public long TotalCount { get; init; }
    public Paginator Paginator { get; init; } = Paginator.Empty;

    public SearchAnswer()
    {
    }
    public SearchAnswer(IReadOnlyList<TEntity> items, long totalCount, Paginator paginator)
    {
        Items = items;
        TotalCount = totalCount;
        Paginator = paginator;
    }
}