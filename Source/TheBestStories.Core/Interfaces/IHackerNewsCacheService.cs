using TheBestStories.Core.Models;

namespace TheBestStories.Core.Interfaces
{
    public interface IHackerNewsCacheService
    {
        IEnumerable<HackerNewsStoryDetailsResponse> GetStoriesFromCache();

        void AddStoriesToCache(IEnumerable<HackerNewsStoryDetailsResponse> hackerNewsStories);
    }
}