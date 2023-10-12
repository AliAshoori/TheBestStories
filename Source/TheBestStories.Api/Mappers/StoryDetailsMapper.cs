using TheBestStories.Api.Responses;
using TheBestStories.Core.Models;

namespace TheBestStories.Api.Mappers
{
    public interface IObjectMapper<TDestination, TSource>
    {
        TDestination Map(TSource source);
    }

    public class GetStoryDetailResponseMapper : IObjectMapper<GetStoryDetailResponse, HackerNewsStoryDetailsResponse>
    {
        public GetStoryDetailResponse Map(HackerNewsStoryDetailsResponse source)
        {
            return new GetStoryDetailResponse(
                source.Title, 
                source.Url, 
                source.By,
                DateTimeOffset.FromUnixTimeSeconds(source.Time).DateTime, 
                source.Score, 
                source.Descendants);
        }
    }
}
