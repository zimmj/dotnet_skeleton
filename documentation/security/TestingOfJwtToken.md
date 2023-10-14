# Testing with Authentication

As soon we add Authentication and Authorization to our application, we need a way to fake the needed jwt token.
This can be done with the help of the [FakeJwtBearer](https://github.com/webmotions/fake-authentication-jwtbearer) library.

It provides a way to add a fake jwt token to the request, which can be used for testing.
With it we can add all needed claims and scopes to the fake token, to make integration testing possible.

### How to use FakeJwtBearer

The fake service is added to the collection in the TestFixture.
After this we can simple call following function on the client.

```csharp
//For simple Authentication:
_client.SetFakeBearerToken(new ExpandoObject());

//To add claims and scopes:
_client.SetFakeBearerToken(
    new Dictionary<string, object>()
    {
        { ClaimTypes.Name, "Test User" },
        { "scp", "houses.read" },
    }
);
```

As you can see, B2C scopes is in the end a claim on the token.
Best is to reference the generated token to see, which claims are needed and what they are called.