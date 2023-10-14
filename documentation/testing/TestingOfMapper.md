# Mapper Testing

As mappings are really common in a project, it is a point where simple mistakes can happen.
You add a new field to the document, but forget to map it on one of the mappings.
An auto mapper usually does not throw any error on compile time.
As it does not check, if all fields are mapped or have an exception.

As of this reason, I tent to test my mappings.

## How to test mappings

Luckily I found a quite simple way, to test the mappings.
Without the need of lot of code.

I am using two packages for it:

- [AutoFixture for XUnit](https://github.com/AutoFixture/AutoFixture)
- [Flunet Assertions](https://fluentassertions.com/)

Combined the test looks like:

```csharp
[Theory, AutoData]
public void MappingTest(House house)
{
    var config = new MapperConfiguration(cfg => cfg.AddProfile<HouseProfile>());
    var mapper = config.CreateMapper();

    var mappedHouse = mapper.Map<HouseDto>(house);

    mappedHouse.Should().BeEquivalentTo(house);
}
```

First the AutoData will populate the house object with random data.
After this we compare the mapped object with the original object, with the help of Fluent Assertions.

The BeEquivalentTo function will compare all properties of the object, and will throw an exception if one of the properties is not mapped.
You can change the behavior of the function with the options attribute, to ignore some properties, if you want to.

For more information see [Be Equivalent To](https://fluentassertions.com/objectgraphs/#beequivalentto).