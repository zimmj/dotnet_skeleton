# Predicates

Predicates are often used to create a filter for a list of objects.
This predicates can get quickly complex and hard to understand.
For this reason I decided to use the PredicateBuilder from the [LINQKit](https://github.com/scottksmith95/LINQKit) library.
It helps to create predicates in a more readable way.

## How to use PredicateBuilder

```csharp
var expressionBuilder = PredicateBuilder.New<HouseDocument>(true);
if (minPrice.HasValue)
    expressionBuilder.And(PriceHigherThanFilter(minPrice.Value));
if (maxPrice.HasValue)
    expressionBuilder.And(PriceLowerThanFilter(maxPrice.Value));
return expressionBuilder;
```

You can easily add new predicates and chain them in a logical way.
The PredicateBuilder will combine them in the end to one expression.

This helps to decouple the filter in different parts, and make it easier to read and test.
If you feel like that a part of the filter need to be tested separately, you can simple extract it to a new function.

## How to test PredicateBuilder

See [HouseQueryFactoryTest.cs](./../../tests/Zimmj.Infrastructure.Tests/Mongo/Houses/HouseQueryFactoryTest.cs) for an example.

Simplified example:
```csharp
var house = new HouseDocument()
{
    Id = "house",
    Name = "house",
    Price = 100
};

var housePriceIsHigherThen = PredicateBuilder.New<HouseDocument>()
            .And(house => house.Price > 50);
var result = expression.Invoke(house);
        
// Assert
result.Should().Be(true);
```

The PredicateBuilder is a simple function, which returns an expression.
This expression can be invoked with the object, which should be tested.
The result is a boolean, which can be asserted.