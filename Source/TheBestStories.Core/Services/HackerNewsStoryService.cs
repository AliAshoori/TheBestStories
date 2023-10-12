using Microsoft.Extensions.Logging;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Models;

namespace TheBestStories.Core.Services
{
    public class HackerNewsStoryService : IHackerNewsStoryService
    {
        private readonly ILogger<HackerNewsStoryService> _logger;
        private readonly IHackerNewsApiClientService _bestStoriesApiClient;

        public HackerNewsStoryService(
            ILogger<HackerNewsStoryService> logger,
            IHackerNewsApiClientService bestStoriesApiClient)
        {
            _logger = logger;
            _bestStoriesApiClient = bestStoriesApiClient;
        }

        public async Task<IEnumerable<HackerNewsStoryDetailsResponse?>> GetBestNStoriesAsync(int requestedNumberOfStories)
        {
            _logger.LogInformation($"{nameof(GetBestNStoriesAsync)} - reading top {requestedNumberOfStories} stories from Hacker News");

            var bestStoryIds = await _bestStoriesApiClient.GetBestStoryIdsAsync();
            var storyTasks = bestStoryIds.Select(id => _bestStoriesApiClient.GetStoryByIdAsync(id));

            try
            {
                var stories = await Task.WhenAll(storyTasks);
                
                if (requestedNumberOfStories > stories.Length)
                {
                    _logger.LogWarning($"The request number of: {requestedNumberOfStories} is greater than the number of stories: {stories.Length}, hence, returning all of them");
                }

                var numbersToTake = requestedNumberOfStories > stories.Length ? stories.Length : requestedNumberOfStories;
                
                return stories.OrderByDescending(x => x.Score).Take(numbersToTake);
            }
            catch (Exception exception)
            {
                var storyTasksException = storyTasks.Where(t => t.Exception != null).Select(t => t.Exception).OfType<AggregateException>();
                foreach (var aggregateException in storyTasksException)
                {
                    _logger.LogError($"{nameof(GetBestNStoriesAsync)} - {aggregateException.Message}");
                }

                return await Task.FromException<IEnumerable<HackerNewsStoryDetailsResponse>>(exception);
            }
        }
    }
}