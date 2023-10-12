namespace TheBestStories.Api.Responses
{
    public record GetStoryDetailResponse(
        string Title,
        string Url,
        string PostedBy,
        DateTime Time,
        long Score,
        long CommentCount
    );
}