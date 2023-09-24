namespace Zimmj.Infrastructure.Mongo.Interfaces;

public interface IIdentifiable<out T>
{
    T Id { get; }
}