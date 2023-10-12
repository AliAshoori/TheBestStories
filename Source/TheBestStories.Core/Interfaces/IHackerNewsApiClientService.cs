using TheBestStories.Core.Models;

namespace TheBestStories.Core.Interfaces
{
    public interface IHackerNewsApiClientService
    {
        Task<HackerNewsStoryDetailsResponse?> GetStoryByIdAsync(long storyId);

        Task<IEnumerable<long>> GetBestStoryIdsAsync();
    }
}
