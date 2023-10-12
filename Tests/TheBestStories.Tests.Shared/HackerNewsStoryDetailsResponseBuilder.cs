using System.Diagnostics.CodeAnalysis;
using TheBestStories.Core.Models;

namespace TheBestStories.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class HackerNewsStoryDetailsResponseBuilder
    {
        public string By { get; private set; } = "Charles Dickens";

        public long DescendantsCount { get; private set; } = 100;

        public long StoryId { get; private set; } = 10;

        public List<long> Kids { get; private set; } = new List<long>();

        public long Score { get; private set; } = 1000;

        public long Time { get; private set; } = 150000;

        public string Title { get; private set; } = "Great Expectation";

        public string Type { get; private set; } = "Book";

        public string Url { get; private set; } = "https://bookland.somewhere";

        public HackerNewsStoryDetailsResponseBuilder WithBy(string by)
        {
            By = by;

            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithDescendantsCount(long count)
        {
            DescendantsCount = count;

            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithStoryId(long storyId)
        {
            StoryId = storyId;

            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithKids(List<long> kids)
        {
            Kids = kids;
            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithTitle(string title)
        {
            Title = title;

            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithType(string type)
        {
            Type = type;

            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithUrl(string url)
        {
            Url = url;

            return this;
        }

        public HackerNewsStoryDetailsResponseBuilder WithScore(long score)
        {
            Score = score;

            return this;
        }

        public HackerNewsStoryDetailsResponse Build()
        {
            return new(By, DescendantsCount, StoryId, Kids, Score, Time, Title, Type, Url);
        }
    }
}