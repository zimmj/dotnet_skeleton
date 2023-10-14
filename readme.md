# C# Microservice Skeleton

This repository describes my idea of a microservice in C#.
It is using different concept, which I used while working with C# in the last year.
I tried to find different sources describing the ideas I had and put the best description together.

The main architectural idea is to use the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

In my implemantation I used following concepts:

- All data gets convert to domain object in the most outer layer.
All other layer are working with the domain object. With this you have a clear understanding, which object is to use when.
And no implementation details are leaking out of the API.
   - Infrastructure layer
   - Rest layer

### Used Libraries and Ideas

- [Result Object](./documentation/resultObject/Result-Object.md) instead of exceptions
- [MediatR]() to create clear and simple single responsibility actions
- [AutoMapper]() to map between domain objects and data objects
- [B2C Authentication]() to authenticate users

### Testing

For me it was important, to create an Application, which has a working integration testing.
With it, we can practice TDD and have a good test coverage.
Additional to integration testing, some unit tests are also implemented.
To further bulster the confidence in the code.

- [Integration Testing]()
- [Mapper testing]()
- [Predicate testing]()


## Hot to get token from the Azure B2C flow

To really get a access token, and not only an id token.
One need to do:

1. Expose an API in the Single page application of Azure b2c
2. Grant the permission to API permission of this Single Page application
3. Grant Admin consent for the API permission
4. The most important step (Which is literally stupidly strange)

   When running flow login from Azure B2C user flow, you need to only select this api scope, not the id scope!

   As probably JWT.ms is reading out the id token, instead of the access token.
   Biggest difference is, int the claims are no scopes shown! ( "scp": "Read.All", is missing)


This is the only way to get a token from the flow.

```javascript