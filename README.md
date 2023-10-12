## The Best Stories API
This is a .net 6 web api application that retrieves the details of the best `n` stories from [Hacker News APIs](https://github.com/HackerNews/API) where `n` is specified by the caller. 

## Considerations during the development
1) Respecting SOLID principles and producing testable coding, for example, utilising mediatr nuget package to better exercise SOC (Separation of Concerns) across the codebase
3) RESTful API patterns with a view of [Richardson maturity model](https://martinfowler.com/articles/richardsonMaturityModel.html) where applicable
4) Make the APIs resilient by implementing retry and circuit breaker policies utilising [Polly](https://github.com/App-vNext/Polly) nuget package

## Assumptions
1) If `n` proves to be bigger than the number of stories back from the Hacker News API, then, the application returns all of them
2) The figures for retry and circuit breaker configurations are just for testing purposes
3) The API does not accepts `n` being equal or less than zero

## The solution structure
It comes with two main sections `source` and `test`. 

`TheBestStories.Api` - this is the API project where the caller endpoint lives. It implements request and request handler pattern wherein the following parts contribute to exercise SOLID more efficiently

1) Requests - each API endpoint (in our case only one) comes with a request object - we do not practice listing a set of primitive parameters and passing them all over the codebase
2) Request Validators - this is where the request object gets validated and if it fails, it will never reach the downstream codebase and the API will return `Bad Request`
3) Response - each request also comes with a response object
4) Request Handler - this is where it clearly binds the request and response objects to the handler and as a result, it massively helps with the `Single Responsibility Principle` and testable coding 
![image](https://github.com/AliAshoori/TheBestStories/assets/7995157/3d8029b4-08e4-4bf9-aca3-612675ae5220)
5) Mappers - this is where the Hacker News API response transforms to the one that must be exposed to the caller - the structure is as per the business requirements

### Note: The request number of the stories (n) comes as a query parameter and not part of the route to better respect the RESTful API naming and addressing conventions.

`TheBestStories.Core` - this is where the actual business logic and application services are hosted.

1) `HackerNewsApiClientService` - the service that calls off the Hacker News API and processes the response
2) `HackerNewsCacheService` - a service class that utilises .net `MemoryCache` to exercise a simple in-memory caching - this .net library is thread-safe
3) `HackerNewsStoryService` - this is the service class that holds all the logic that needs to be implemented against the response received from Hacker News API
4) `HackerNewsApiClientPolicyService` - this is where the `Polly` policies are implemented
5) `HackerNewsApiClientPolicyExecutorService` - and finally the service class that executes the Polly policies

Having the design above not only respects the SRP but also modularises the services and encourages a pattern for future development.

The test projects go after each of the Api and Core projects respectively. 

## How to run the application
1) Make sure .net 6 is installed on the target machine
2) Use Visual Studio 2022 or a version that supports .net 6 and open up the `.sln` file
3) right click on the solution and rebuild the application
   ![image](https://github.com/AliAshoori/TheBestStories/assets/7995157/32fda4de-bb25-4bcf-9b88-a3a07d35fe57)
5) Hit F5 and the swagger page will appear, you can use the Swagger to try the API
![image](https://github.com/AliAshoori/TheBestStories/assets/7995157/97673465-cb2c-4c85-8116-77f11befa197)

The API runs at port: `7054` as shown in the screenshot above. Alternatively, you can use `postman` to fire off the API as well: `https://localhost:7054/api/stories/?topNStories=10`

## Postman results

1) When the request number of the stories is invalid, that is, zero or negative
![image](https://github.com/AliAshoori/TheBestStories/assets/7995157/336cb0b8-43ca-4d4a-8ce6-a4123f8571ac)
2) Happy scenario (no cache)
![image](https://github.com/AliAshoori/TheBestStories/assets/7995157/1cb1df2e-9153-413a-8988-e01fed99438b)
3) Happy scenario (with cache)
![image](https://github.com/AliAshoori/TheBestStories/assets/7995157/bb871ccb-b9c8-473e-8695-e2fbdec296ab)


## Potential improvements
1) More code coverage including integration testings
2) Dockerising the application
3) Performance/stress testing

## Extra miles - preparing for production
1) API Security - OAUth2 could be a good option
2) Azure API Gateway - making the API resilient and practicing security through inbound traffic
3) Firewalls and VMs to secure the APIs better - whitelisting only the expected traffic based on the business requirements
4) Consider a better caching strategy, for example, Redis caching
5) CI/CD
