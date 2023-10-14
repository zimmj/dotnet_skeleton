# Integration Testing

Integration testing is one part of the testing pyramid.
The pyramid is a good way to visualize, the corresponding cost and amount which is often used.

![Testing Pyramid](./images/testing-pyramid.png)

As you can see, the amount of unit tests is the highest, as they are the cheapest to write and maintain.
The amount of testing decreases, how higher we are coming in the system.

This is only partially true. As with modern approach, as clean architecture, the amount of integration testing should be
increased.
In my opinion it should surpass the amount of unit tests.
As we are usually working with much smaller services, which should be easier to integration test.

The biggest hurdle of integration testing is the first setup, that you have the complete application running but are
still able to mock some parts of it.
Many back-end frameworks are build with this in mind.
I was expecting the same from .NET Core, but was disappointed.
The documentation is not really helpful, and the examples are not really working.

The provided example in
the [Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0),
was not working for me
As I needed a way to have the mongo db as in memory database, and not a real one.

I found
an [example](http://www.jonathanawotwi.com/2021/06/mongodb-integration-testing-in-net-core-mongo2go-vs-mongodb-docker-instance/),
which described the use of mongo2go.
The implementation did work mostly, but the injection of the generated mongo db connection string was not working.

So I needed to find a way to inject the connection string into the application. And found it in a github issue.
Unfortunately I forgot the source.

### How to setup integration testing

My implementation can be seen in [TestFixture.cs](./../../tests/Zimmj.Integration.Tests/Fixtures/TestFixture.cs)

It needs the startup class of the application, we want integration test and provides with the help of the Database
Fixture the whole application.
Pay attention, that this fixture is creating a singleton of the application, and therefore each client has the same
settings.
This is made to save computation time and resources.

The applications services can be configured in the single test file:

```csharp
testFixture.ConfigureTestOptions(options =>
{
    options.ConfigureDatabase("HouseControllerTest");
    options.ConfigureWebHost(services =>
    {
        //Add Mocked services here for injection
    });
});
```

This injection will be run, after the original startup class was run.

The test fixture provides a client, which can be used to call the application:
This client is as well a singleton, to avoid multiple calls to the startup and registration of the services.

```csharp
var client = testFixture.CreateClient();
```

#### Trouble with Mongo2Go

As Mongo2Go chooses an open port by random, each time the connection string has a different port.
Therefore the settings need to be changed, before the creation of the mongo collection is called.

Unfortunately is the normal hook to late:

```csharp
// Before TStartup ConfigureServices.
builder.ConfigureServices(collection =>
{
collection.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
});
```

Normally the configuration should be added here.
But since the newer version, the configuration is already called before this hook.

Therefore we need to use another method, which looks like:

```csharp
 var testConfigurationBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new List<KeyValuePair<string, string?>>
                {
                    new("MongoInformation:ConnectionString", DatabaseFixture?.ConnectionString ?? ""),
                    new("MongoInformation:DatabaseName", DatabaseFixture?.DatabaseName ?? "")
                }).Build();

 builder.UseEnvironment(_hostingEnvironment)
                    .UseConfiguration(testConfigurationBuilder)
```

this will override the configuration with the new connection string, which was set before.

#### Multiple StartUp calls to global registration

Even though that the application should get disposed of, some settings set in the global registration are not removed.
This can lead to different thrown exceptions, as the settings are not valid anymore.

For example the following registration need a catch.
As in the second call, there is already another class registered.

```csharp
 try
 {
     BsonSerializer.TryRegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
     BsonSerializer.TryRegisterSerializer(typeof(decimal?),
         new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
 }
 catch (Exception e)
 {
     Console.WriteLine(e);
 }
```

Unfortunately I did not find a better solution for this problem.


See also:
- [Testing with Authentication and Authorization](./../security/TestingOfJwtToken.md)