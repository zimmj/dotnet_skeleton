using MongoDB.Bson.Serialization.Attributes;
using Zimmj.Core.Houses;
using Zimmj.Infrastructure.Mongo.CrossCutting;
using Zimmj.Infrastructure.Mongo.Interfaces;

namespace Zimmj.Infrastructure.Mongo.Houses;

public class HouseDocument : BaseDocument<HouseDocument, House>, IIdentifiable<string>
{

    private string? _id;
    
    [BsonId]
    public string Id
    {
        get
        {
            if (_id is null) return Name;
            return _id;
        }
        set => _id = value;
    }

    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
}