using Microsoft.Extensions.Logging;
using System.Text.Json;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Models;

namespace TheBestStories.Core.Services
{
    public class HackerNewsApiClientService : IHackerNewsApiClientService
    {
        private readonly ILogger<HackerNewsApiClientService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHackerNewsApiClientPolicyExecutorService _apiPolicyExecutor;

        public HackerNewsApiClientService(
            ILogger<HackerNewsApiClientService> logger,
            HttpClient httpClient,
            IHackerNewsApiClientPolicyExecutorService apiPolicyExecutor)
        {
            _logger = logger;
            _httpClient = httpClient;
            _apiPolicyExecutor = apiPolicyExecutor;
        }

        public async Task<IEnumerable<long>> GetBestStoryIdsAsync()
        {
            _logger.LogInformation($"{nameof(GetBestStoryIdsAsync)} - fetching stories from GitHub");

            var gitHubResponse =
                await _apiPolicyExecutor.ExecuteAsync(async () => await _httpClient.GetAsync("beststories.json"));

            for (var i = 0; i <= 10000; i++) 
            {
                _logger.LogInformation($"{i}");
            }
            
            gitHubResponse.EnsureSuccessStatusCode();

            var data = await gitHubResponse.Content.ReadAsStreamAsync();

            if (data == null || data.Length == 0)
            {
                _logger.LogInformation($"{nameof(GetBestStoryIdsAsync)} - received no data from GitHub");

                return Enumerable.Empty<long>();
            }

            return JsonSerializer.Deserialize<IEnumerable<long>>(data) ?? Enumerable.Empty<long>();
        }

        public async Task<HackerNewsStoryDetailsResponse?> GetStoryByIdAsync(long storyId)
        {
            _logger.LogInformation($"{nameof(GetStoryByIdAsync)} - reading story with id: {storyId} from Hacker News API");

            var gitHubResponse = await _httpClient.GetAsync($"item/{storyId}.json");

            gitHubResponse.EnsureSuccessStatusCode();

            var data = await gitHubResponse.Content.ReadAsStreamAsync();

            if (data == null || data.Length == 0)
            {
                _logger.LogInformation($"{nameof(GetStoryByIdAsync)} - received no data from GitHub");

                return null;
            }

            return JsonSerializer.Deserialize<HackerNewsStoryDetailsResponse?>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
