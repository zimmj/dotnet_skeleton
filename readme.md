# C# Microservice Skeleton

This repository describes my idea of a microservice in C#.
It is using different concept, which I used while working with C# in the last year.
I tried to find different sources describing the ideas I had and put the best description together.

The main architectural idea is to use the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

In my implementation I used following concepts:

- All data gets convert to domain object in the most outer layer.
All other layer are working with the domain object. With this you have a clear understanding, which object is to use when.
And no implementation details are leaking out of the API.
   - Infrastructure layer
   - Rest layer

### Used Libraries and Ideas

- [Result Object](./documentation/resultObject/Result-Object.md) instead of exceptions
- [MediatR](./documentation/mediator/Mediator.md) to create clear and simple single responsibility actions
- [AutoMapper](./documentation/mapping/Auto-Mapping.md) to map between domain objects and data objects
- [B2C Authentication](./documentation/security/B2C.md) to authenticate users
- [Input Valudation](), validate user input on handler pipeline (to be implemented)
- [Logging](), change logging to json, add logging in handler pipeline. (to be implemented)

### Testing

For me it was important, to create an Application, which has a working integration testing.
With it, we can practice TDD and have a good test coverage.
Additional to integration testing, some unit tests are also implemented.
To further bulster the confidence in the code.

- [Integration Testing](./documentation/testing/IntegrationTesting.md)
- [Testing Authentication and Authorization](./documentation/testing/IntegrationTesting.md)
- [Mapper Testing](./documentation/testing/TestingOfMapper.md)
- [Predicate Testing](./documentation/testing/PredicateTesting.md)


### Docker build //todo

- add docker file with test-runner and explain how to use it