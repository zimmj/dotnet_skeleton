using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Zimmj.Core.CrossCutting.ResultExtensions.Errors;
using Zimmj.Core.CrossCutting.ResultExtensions.Successes;

namespace Zimmj.Presentation.CrossCutting.ResultExtensions;

public static class ResultTransformer
{
    public static ActionResult<TEntity> ToActionResult<TEntity>(
        this Result<TEntity> result,
        string? pathToEntity = null
    )
    {
        if (result.IsSuccess)
        {
            return ToSuccessActionResult(result, pathToEntity);
        }

        return ToBadActionResult(result);
    }

    private static ActionResult ToBadActionResult<TEntity>(Result<TEntity> result)
    {
        switch (result.Errors.FirstOrDefault())
        {
            case EntityNotFoundError:
                return new NotFoundResult();
        }

        return new BadRequestObjectResult(result.Errors);
    }

    private static ActionResult<TEntity> ToSuccessActionResult<TEntity>(this Result<TEntity> result,
        string? pathToEntity = null)
    {
        switch (result.Successes.FirstOrDefault())
        {
            case ChangeAccepted:
                return new OkObjectResult(result.Value);
            case EntityCreated:
                return new CreatedResult(pathToEntity ?? "", result.Value);
        }

        return new OkObjectResult(result.Value);
    }

    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return ToSuccessActionResult(result);
        }

        return ToBadActionResult(result);
    }

    private static ActionResult ToBadActionResult(Result result)
    {
        switch (result.Errors.FirstOrDefault())
        {
            case EntityNotFoundError:
                return new NotFoundResult();
        }

        return new BadRequestObjectResult(result.Errors);
    }

    private static ActionResult ToSuccessActionResult(this Result result)
    {
        switch (result.Successes.FirstOrDefault())
        {
            case ChangeAccepted:
                return new OkResult();
        }

        return new OkResult();
    }

    public static ActionResult ToActionResult(this Result result,
        string actionName, string controllerName, object routeValues
    )
    {
        if (result.IsSuccess)
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, "");
        }

        return ToBadActionResult(result);
    }

    public static ActionResult<TEntity> ToActionResult<TEntity>(this Result<TEntity> result,
        string actionName, string controllerName, object routeValues
    )
    {
        if (result.IsSuccess)
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, result.Value);
        }

        return ToBadActionResult(result);
    }
}