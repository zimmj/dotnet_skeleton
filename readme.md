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