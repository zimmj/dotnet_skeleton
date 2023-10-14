using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Zimmj.Core.CrossCutting.ResultExtensions.Errors;
using Zimmj.Core.CrossCutting.ResultExtensions.Successes;

namespace Zimmj.Rest.CrossCutting.ResultExtensions;

public static class ResultTransformer
{
    public static ActionResult<TEntity> ToActionResult<TEntity>(
        this Result<TEntity> result
    )
    {
        return result.IsSuccess switch
        {
            true => ToSuccessActionResult(result),
            _ => ToBadActionResult(result)
        };
    }

    private static ActionResult ToBadActionResult<TEntity>(Result<TEntity> result)
    {
        return result.Errors.FirstOrDefault() switch
        {
            EntityNotFoundError => new NotFoundResult(),
            _ => new BadRequestObjectResult(result.Errors)
        };
    }

    private static ActionResult<TEntity> ToSuccessActionResult<TEntity>(this Result<TEntity> result)
    {
        return result.Successes.FirstOrDefault() switch
        {
            ChangeAccepted => new AcceptedResult(),
            EntityCreatedAt createdAt => createdAt.ToCreatedAtActionResult(result.Value),
            _ => new OkObjectResult(result.Value)
        };
    }

    public static ActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess switch
        {
            true => ToSuccessActionResult(result),
            _ => ToBadActionResult(result)
        };
    }

    private static ActionResult ToBadActionResult(Result result)
    {
        return result.Errors.FirstOrDefault() switch
        {
            EntityNotFoundError => new NotFoundResult(),
            _ => new BadRequestObjectResult(result.Errors)
        };
    }

    private static ActionResult ToSuccessActionResult(this Result result)
    {
        return result.Successes.FirstOrDefault() switch
        {
            ChangeAccepted => new OkResult(),
            EntityCreatedAt createdAt => createdAt.ToCreatedAtActionResult(),
            _ => new OkResult()
        };
    }
    
    
    private static ActionResult ToCreatedAtActionResult<T>(this EntityCreatedAt entityCreatedAt, T value)
    {
        return new CreatedAtActionResult(
            entityCreatedAt.ActionName,
            entityCreatedAt.ControllerName,
            entityCreatedAt.RouteValues,
            value
        );
    }

    private static ActionResult ToCreatedAtActionResult(this EntityCreatedAt entityCreatedAt)
    {
        return new CreatedAtActionResult(
            entityCreatedAt.ActionName,
            entityCreatedAt.ControllerName,
            entityCreatedAt.RouteValues,
            null
        );
    }
}