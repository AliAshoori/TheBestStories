using MediatR;
using TheBestStories.Api.Mappers;
using TheBestStories.Api.Requests;
using TheBestStories.Api.Responses;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Models;

namespace TheBestStories.Api.RequestHandlers
{
    public class GetBestStoriesRequestHandler : IRequestHandler<GetBestStoriesRequest, IEnumerable<GetStoryDetailResponse>>
    {
        private readonly IHackerNewsCacheService _cacheService;
        private readonly IHackerNewsStoryService _hackerNewsService;
        private readonly IObjectMapper<GetStoryDetailResponse, HackerNewsStoryDetailsResponse> _mapper;
        
        public GetBestStoriesRequestHandler(
            IHackerNewsCacheService cacheService,
            IHackerNewsStoryService hackerNewsService,
            IObjectMapper<GetStoryDetailResponse, HackerNewsStoryDetailsResponse> mapper)
        {
            _cacheService = cacheService;
            _hackerNewsService = hackerNewsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetStoryDetailResponse>> Handle(
            GetBestStoriesRequest request,
            CancellationToken cancellationToken)
        {
            // 1. read data from cache if available there
            var storiesFromCache =  _cacheService.GetStoriesFromCache();
            if (storiesFromCache.Count() >= request.RequestedNumberOfStories)
            {
                return storiesFromCache
                    .Take(request.RequestedNumberOfStories)
                    .Select(hackerNewsStory => _mapper.Map(hackerNewsStory));
            }

            // 2. read data from Hacker News API as it was not available in the cache
            var stories = await _hackerNewsService.GetBestNStoriesAsync(request.RequestedNumberOfStories);

            // 3. update the cache with the new data
            _cacheService.AddStoriesToCache(stories);

            // 4. return the data
            return stories.Select(hackerNewsStory => _mapper.Map(hackerNewsStory));
        }
    }
}