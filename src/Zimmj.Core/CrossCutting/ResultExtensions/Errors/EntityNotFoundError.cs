using FluentResults;

namespace Zimmj.Core.CrossCutting.ResultExtensions.Errors;

public class EntityNotFoundError : Error
{
    public EntityNotFoundError(string nameOfType) : base(
        $"Entity of type ${nameOfType} not found.")
    {
    }
}