using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Services;

namespace TheBestStories.Core.Tests.HackerNewsApiClientServiceTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetBestStoryIdsTests
    {
        [TestMethod]
        public async Task GetBestStoryIdsAsync_HappyScenario_ReturnsStoryIds()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsApiClientService>>();
            var storyIds = new[] { 1, 2, 3, 4, };

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            var setupApiRequest = mockHttpHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );

            var hackerNewsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(storyIds))
            };

            setupApiRequest.ReturnsAsync(hackerNewsResponse);

            var mockPolicyExecutor = new Mock<IHackerNewsApiClientPolicyExecutorService>();
            mockPolicyExecutor
                .Setup(m => m.ExecuteAsync(It.IsAny<Func<Task<HttpResponseMessage>>>()))
                .ReturnsAsync(hackerNewsResponse);

            var mockHttpClient = new HttpClient(mockHttpHandler.Object) { BaseAddress = new Uri("https://hello-there") };

            var service = new HackerNewsApiClientService(mockLogger.Object, mockHttpClient, mockPolicyExecutor.Object);

            // Act
            var result = await service.GetBestStoryIdsAsync();

            // Assert
            result.Should().BeEquivalentTo(storyIds);
        }

        [TestMethod]
        public void GetBestStoryIdsAsync_WithApiException_ThrowsHttpRequestException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsApiClientService>>();
            var storyIds = new[] { 1, 2, 3, 4, };

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            var setupApiRequest = mockHttpHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );

            setupApiRequest.ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

            var mockPolicyExecutor = new Mock<IHackerNewsApiClientPolicyExecutorService>();

            var mockHttpClient = new HttpClient(mockHttpHandler.Object) { BaseAddress = new Uri("https://hello-there") };

            var service = new HackerNewsApiClientService(mockLogger.Object, mockHttpClient, mockPolicyExecutor.Object);

            // Act
            var result = async () => await service.GetBestStoryIdsAsync();

            // Assert
            result.Should().ThrowAsync<HttpRequestException>();
        }

        [TestMethod]
        public async Task GetBestStoryIdsAsync_WithNoDataFromApi_ReturnsEmptyArray()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HackerNewsApiClientService>>();
            var storyIds = Enumerable.Empty<long>();

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            var setupApiRequest = mockHttpHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                );

            var hackerNewsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(storyIds))
            };

            setupApiRequest.ReturnsAsync(hackerNewsResponse);

            var mockPolicyExecutor = new Mock<IHackerNewsApiClientPolicyExecutorService>();
            mockPolicyExecutor
                .Setup(m => m.ExecuteAsync(It.IsAny<Func<Task<HttpResponseMessage>>>()))
                .ReturnsAsync(hackerNewsResponse);

            var mockHttpClient = new HttpClient(mockHttpHandler.Object) { BaseAddress = new Uri("https://hello-there") };

            var service = new HackerNewsApiClientService(mockLogger.Object, mockHttpClient, mockPolicyExecutor.Object);

            // Act
            var result = await service.GetBestStoryIdsAsync();

            // Assert
            result.Should().BeEquivalentTo(storyIds);
        }
    }
}