using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System.Net;
using System.Text;
using TheBestStories.Core.Interfaces;

namespace TheBestStories.Core.Services
{
    public class HackerNewsApiClientPolicyService: IHackerNewsApiClientPolicyService
    {
        private readonly ILogger<HackerNewsApiClientPolicyService> _logger;
        private readonly IOptions<AppOptions> _options;

        public HackerNewsApiClientPolicyService(
            ILogger<HackerNewsApiClientPolicyService> logger,
            IOptions<AppOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public IAsyncPolicy<HttpResponseMessage> AddCircuitBreaker()
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(IsTransientError)
                .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: _options.Value.HackerNewsApi.HandledEventsAllowedBeforeBreaking,
                durationOfBreak: TimeSpan.FromSeconds(_options.Value.HackerNewsApi.CircuitBreakDurationInSec),
                onBreak: (httpResponseMessage, timeSpan) =>
                {
                    var errorMessage = new StringBuilder("The circuit is now on-break ");
                    errorMessage.AppendLine($"Hacker News API status code: {httpResponseMessage.Result.StatusCode} ");
                    errorMessage.AppendLine($"Hacker News API content: {httpResponseMessage.Result.Content?.ReadAsStringAsync().Result} ");

                    _logger.LogWarning($"{errorMessage}");
                },
                onReset: () => 
                {
                    _logger.LogInformation($"The circuit is now getting reset");
                });

            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> AddRetryPolicy()
        {
            var policy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(IsTransientError)
                .WaitAndRetryAsync(
                retryCount: _options.Value.HackerNewsApi.RetryCount,
                sleepDurationProvider: (retryCount, response, context) =>
                {
                    var errorMessage = new StringBuilder("sleepDuration running - ");
                    errorMessage.Append($"Calculating SleepDuration for retry count: {retryCount}");

                    var hackerNewsSuggestedRetryAfter = response.Result?.Headers.RetryAfter?.Delta.Value;

                    var sleepDuration = hackerNewsSuggestedRetryAfter != null ?
                                        hackerNewsSuggestedRetryAfter :
                                        TimeSpan.FromSeconds(_options.Value.HackerNewsApi.RetryWaitDurationInSec);

                    return sleepDuration.Value;
                },
                onRetryAsync: (response, span, retryCount, context) =>
                {
                    var exceptionMessage = response.Exception != null ? response.Exception.Message : "<null>";
                    
                    var errorMessage = new StringBuilder("onRetryAsync running - ");
                    errorMessage.Append($" Exception: {exceptionMessage}");
                    errorMessage.Append($"Hacker News API status code: {response.Result.StatusCode} ");
                    errorMessage.Append($"Retry count: {retryCount} ");
                    errorMessage.Append($"Time to wait in seconds: {span.TotalSeconds}");

                    _logger.LogWarning($"{errorMessage}");

                    return Task.CompletedTask;

                });

            return policy;
        }

        static bool IsTransientError(HttpResponseMessage httpResponse)
        {
            return httpResponse.StatusCode == HttpStatusCode.TooManyRequests ||
                httpResponse.StatusCode == HttpStatusCode.RequestTimeout ||
                (int)httpResponse.StatusCode >= (int)HttpStatusCode.InternalServerError;
        }
    }
}