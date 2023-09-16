namespace Zimmj.Infrastructure.Mongo.Interfaces;

internal interface IIdentifiable<out T>
{
    T Id { get; }
}