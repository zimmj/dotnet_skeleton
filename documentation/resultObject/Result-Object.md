# Result object instead of exceptions

## Idea [ Exceptions for Flow Control by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)
The idea in quick, is that we have real hard exceptions, which are not foreseen.
Anything else, which can be foreseen in the flow, should be handled by an other object.
In this case the result object.

## Why [FluentResult](https://github.com/altmann/FluentResults) 

I decide to use FluentResult, as it is a good implementation of the result class.

- It has a clear state
- Simple to map to other result objects
- Has "explanation" why it's a success or failure
- It has a fluent api, this kind of chaining I like in programming.

### How to create a result object

```csharp
Result.Ok(houseDocument.ToEntity())

// create a result which indicates failure
Result.Fail<House>(new EntityNotFoundError(nameof(House)));
```

### How to map to other result objects

```csharp
result.Map(SearchAnswerHouseMapper.FromEntity).ToActionResult()
```

### How to use the "explanation" in the Result

As you saw on creation of an result object, you can add an explanation, either Success or Error object.
This class can then be used, to further decide how to map the result to it's corresponding action result.

```csharp
return result.Errors.FirstOrDefault() switch
{
    EntityNotFoundError => new NotFoundResult(),
    _ => new BadRequestObjectResult(result.Errors)
};
```

With this, we have a simple way, to map the result object to the corresponding action result.
This can be enhanced with as many different error types as needed for the application.

The same can be done, for the success object.

```csharp
return  result.Successes.FirstOrDefault() switch
{
    ChangeAccepted => new AcceptedResult(),
    EntityCreatedAt createdAt => createdAt.ToCreatedAtActionResult(result.Value),
    _ => new OkObjectResult(result.Value)
};
```

With this we have a clear, reusable flow control, which can be used in all services.
Changes like this, are mostly gathered in a shared library, which can be used in all services.