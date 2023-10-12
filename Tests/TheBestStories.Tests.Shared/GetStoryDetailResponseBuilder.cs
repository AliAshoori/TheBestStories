using System.Diagnostics.CodeAnalysis;
using TheBestStories.Api.Responses;

namespace TheBestStories.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class GetStoryDetailResponseBuilder
    {
        public string Title { get; private set; } = "Great Expectations";

        public string Url { get; private set; } = "https://bookland.somewhere";

        public string PostedBy { get; private set; } = "Charles Dickens";

        public DateTime Time { get; private set; } = new DateTime(2023, 10, 10);

        public long Score { get; private set; } = int.MaxValue;

        public long CommentCount { get; private set; } = int.MaxValue;

        public GetStoryDetailResponseBuilder WithTitle(string title)
        {
            Title = title;
            
            return this;
        }

        public GetStoryDetailResponseBuilder WithUrl(string url)
        {
            Url = url;
            
            return this;
        }

        public GetStoryDetailResponseBuilder WithPostedBy(string postedBy)
        {
            PostedBy = postedBy;

            return this;
        }

        public GetStoryDetailResponseBuilder WithTime(DateTime time)
        {
            Time = time;

            return this;
        }

        public GetStoryDetailResponseBuilder WithScore(long score)
        {
            Score = score;

            return this;
        }

        public GetStoryDetailResponseBuilder WithCommentCount(long commentCount)
        {
            CommentCount = commentCount;

            return this;
        }

        public GetStoryDetailResponse Build()
        {
            return new(Title, Url, PostedBy, Time, Score, CommentCount);
        }
    }
}