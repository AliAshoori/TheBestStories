using Polly;

namespace TheBestStories.Core.Interfaces
{
    public interface IHackerNewsApiClientPolicyService
    {
        IAsyncPolicy<HttpResponseMessage> AddCircuitBreaker();

        IAsyncPolicy<HttpResponseMessage> AddRetryPolicy();
    }
}
