using MediatR;
using TheBestStories.Api.Responses;

namespace TheBestStories.Api.Requests
{
    public record GetBestStoriesRequest(int RequestedNumberOfStories) : IRequest<IEnumerable<GetStoryDetailResponse>>;
}
