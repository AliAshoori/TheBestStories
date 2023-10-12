using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TheBestStories.Api.Mappers;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Api.Tests.Mappers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetStoryDetailResponseMapperTests
    {
        [TestMethod]
        public void Map_HappyScenario_ReturnsGetStoryDetailResponse()
        {
            // Arrange
            var hackerNewsResponse = new HackerNewsStoryDetailsResponseBuilder().Build();
            var expected = new GetStoryDetailResponseBuilder()
                .WithCommentCount(hackerNewsResponse.Descendants)
                .WithTime(DateTimeOffset.FromUnixTimeSeconds(hackerNewsResponse.Time).DateTime)
                .WithPostedBy(hackerNewsResponse.By)
                .WithScore(hackerNewsResponse.Score)
                .WithTitle(hackerNewsResponse.Title)
                .WithUrl(hackerNewsResponse.Url)
                .Build();

            var mapper = new GetStoryDetailResponseMapper();

            // Act
            var result = mapper.Map(hackerNewsResponse);

            // Assert
            result.Should().Be(expected);
        }
    }
}
