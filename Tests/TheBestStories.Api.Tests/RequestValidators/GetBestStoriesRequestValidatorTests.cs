using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using TheBestStories.Api;
using TheBestStories.Api.RequestValidators;
using TheBestStories.Tests.Shared;

namespace TheBestStories.Apis
{

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GetBestStoriesRequestValidatorTests
    {
        [TestMethod]
        public void Validate_HappyScenario_ReturnsNoError()
        {
            // Arrange
            var request = new GetBestStoriesRequestBuilder().Build();
            var validator = new GetBestStoriesRequestValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(0);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void Validate_WithInvalidRequestedNumber_ReturnsError(int requestedNumberOfStories)
        {
            // Arrange
            var request = new GetBestStoriesRequestBuilder()
                .WithRequestedNumberOfStories(requestedNumberOfStories)
                .Build();
            
            var validator = new GetBestStoriesRequestValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be(Strings.RequestedNumberOfStoriesErrorMessage);
        }
    }
}