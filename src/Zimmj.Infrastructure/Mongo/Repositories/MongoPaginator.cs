using Zimmj.Core.CrossCutting.Search;
using Zimmj.Infrastructure.Mongo.CrossCutting;

namespace Zimmj.Infrastructure.Mongo.Repositories;

public class MongoPaginator : BaseDocument<MongoPaginator, Paginator>
{
    public int Skip { get; init; }
    public int Take { get; init; }
}