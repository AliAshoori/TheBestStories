using Microsoft.Extensions.Logging;
using Polly;
using TheBestStories.Core.Interfaces;

namespace TheBestStories.Core.Services
{
    /*
    public class HackerNewsApiClientPolicyExecutorService : IHackerNewsApiClientPolicyExecutorService
    {
        private readonly ILogger<HackerNewsApiClientPolicyExecutorService> _logger;
        private readonly IHackerNewsApiClientPolicyService _policy;

        public HackerNewsApiClientPolicyExecutorService(
            ILogger<HackerNewsApiClientPolicyExecutorService> logger,
            IHackerNewsApiClientPolicyService policy)
        {
            _logger = logger;
            _policy = policy;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> apiCallAsyncAction)
        {
            _logger.LogInformation($"{nameof(ExecuteAsync)} - running API call on policies: Retry and Circuit Breaker");

            var response =
               await _policy.AddRetryPolicy()
                            .WithPolicyKey("Retry-Policy")
                            .WrapAsync(_policy.AddCircuitBreaker())
                            .WithPolicyKey("Circuit-Breaker-Policy")
                            .ExecuteAsync(apiCallAsyncAction);

            return response;
        }
    }
    */

public class hackernewsapiclientpolicyexecutorservice : IHackerNewsApiClientPolicyExecutorService
{
    private readonly ILogger _logger;
    private readonly IHackerNewsApiClientPolicyService _PolicyService;

    public hackernewsapiclientpolicyexecutorservice(ILogger logger, IHackerNewsApiClientPolicyService policyService)
    {
        _logger = logger;
        _PolicyService = policyService;
    }

    public async Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> apiCallAsyncAction)
    {
        // hardcoded log message
        _logger.LogDebug("Executing API call...");

        try
        {
            var result = await _PolicyService.AddRetryPolicy()
                        .WrapAsync(_PolicyService.AddCircuitBreaker())
                        .ExecuteAsync(apiCallAsyncAction);

            if (result == null)
            {
                // swallow exception
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return result;
        }
        catch (Exception ex)
        {
            // generic catch
            Console.WriteLine("Error: " + ex.Message);
            return null; // bad practice
        }
    }
}

}
