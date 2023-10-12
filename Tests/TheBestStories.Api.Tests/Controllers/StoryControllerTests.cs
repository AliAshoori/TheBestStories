using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using TheBestStories.Api.Controllers;
using TheBestStories.Api.Responses;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Apis
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class StoryControllerTests
    {
        [TestMethod]
        public async Task GetBestStoriesAsync_HappyScenario_ReturnsOk()
        {
            // Arrange
            const int requestedNumber = 1;
            var request = new GetBestStoriesRequestBuilder()
                .WithRequestedNumberOfStories(requestedNumber)
                .Build();

            var expected = new List<GetStoryDetailResponse>
            {
                new GetStoryDetailResponseBuilder().Build()
            };

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(request, default)).ReturnsAsync(expected);

            var controller = new StoryController(mockMediator.Object);

            // Act
            var result = (await controller.GetBestStoriesAsync(request.RequestedNumberOfStories)).Result as OkObjectResult;

            // Assert
            result?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result?.Value.Should().Be(expected);
        }

        [TestMethod]
        public void GetBestStoriesAsync_WithApplicationError_ThrowsInternalServerError()
        {
            // Arrange
            const int requestedNumber = 1;
            var request = new GetBestStoriesRequestBuilder()
                .WithRequestedNumberOfStories(requestedNumber)
                .Build();

            var expected = new List<GetStoryDetailResponse>
            {
                new GetStoryDetailResponseBuilder().Build()
            };

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Send(request, default)).Throws<InvalidOperationException>();

            var controller = new StoryController(mockMediator.Object);

            // Act
            var result = async () => await controller.GetBestStoriesAsync(request.RequestedNumberOfStories);

            // Assert
            result?.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}