using TheBestStories.Core.Models;

namespace TheBestStories.Core.Interfaces
{
    public interface IHackerNewsStoryService
    {
        public Task<IEnumerable<HackerNewsStoryDetailsResponse?>> GetBestNStoriesAsync(int requestedNumberOfStories);
    }
}
