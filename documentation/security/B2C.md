# Authorization with [B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/overview)

B2C is a service from Azure, which allows you to create a login flow for your application.
It is a simple way, to have a quick and well integrated OAuth2 flow.
The provided tutorial to setup B2C is quite good. [Tutorial](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-tenant)

### Adding Authentication and Authorization to a controller

I am using the setup for an single page application with a simple login flow, as described in the above tutorial.
Next follow [Enable Authentication](https://learn.microsoft.com/en-us/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient)
so we can add the properties to our controller.
Don't forget to add and enable the scope! [How to add Scope](https://learn.microsoft.com/en-us/azure/active-directory-b2c/configure-authentication-sample-spa-app#step-22-configure-scopes).

Now we can get a token from the flow, pay attention to select just the needed scope [see](#hidden-troubles-with-b2c), and use it to call our API.
```csharp
[Authorize]
[ApiController]
[RequiredScope("houses.read")]
[Route("api/[controller]")]
public class HouseController : ControllerBase
{
}
```

The scope need to be provided bey the access token.
If the token has not the scope, the request will return a 403.

## Hidden troubles with B2C

As I was working with B2C, I found some high ups.
One of them is, that Authorization is not necessarily needed to perform Authentication.
For some services, any logged in user, should be able to call it.

But if you don't add a scope to B2C, you will only get an id token, which is not enough to call the service.
The error thrown by the client is not really helpful, as it only says, that the token is invalid.

To get a access token, you need to add a scope to the B2C flow, but as well remove the scope for the id token.

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
