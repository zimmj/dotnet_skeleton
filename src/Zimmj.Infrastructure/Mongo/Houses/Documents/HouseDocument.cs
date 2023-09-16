using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Zimmj.Infrastructure.Mongo.Interfaces;

namespace Zimmj.Infrastructure.Mongo.Houses.Documents;

public class HouseDocument : IIdentifiable<string>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}