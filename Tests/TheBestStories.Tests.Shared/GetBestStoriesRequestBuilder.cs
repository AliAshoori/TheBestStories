using System.Diagnostics.CodeAnalysis;
using TheBestStories.Api.Requests;

namespace TheBestStories.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class GetBestStoriesRequestBuilder
    {
        public int RequestedNumberOfStories { get; private set; } = 1;

        public GetBestStoriesRequestBuilder WithRequestedNumberOfStories(int requestedNumberOfStories)
        {
            RequestedNumberOfStories = requestedNumberOfStories;

            return this;
        }

        public GetBestStoriesRequest Build() => new(RequestedNumberOfStories);
    }
}