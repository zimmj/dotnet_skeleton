using FluentResults;

namespace Zimmj.Core.CrossCutting.ResultExtensions.Successes;

public class EntityCreatedAt : Success
{
    public string ActionName { get; }
    public string ControllerName { get; }
    public object RouteValues { get; }
    public EntityCreatedAt( string actionName, string controllerName, object routeValues)
    {
        ActionName = actionName;
        ControllerName = controllerName;
        RouteValues = routeValues;
    }
}