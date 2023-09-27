namespace Zimmj.Core.CrossCutting.Search;

public class Paginator
{
    public int Skip { get; init; }
    public int Take { get; init; }

    public Paginator()
    {
    }

    public Paginator(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
    
    public static Paginator Empty => new();
}