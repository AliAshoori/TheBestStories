using System.Text.Json.Serialization;

namespace TheBestStories.Core.Models
{
    public record HackerNewsStoryDetailsResponse(
        [property: JsonPropertyName("by")] string By,
        [property: JsonPropertyName("descendants")] long Descendants,
        [property: JsonPropertyName("id")] long Id,
        [property: JsonPropertyName("kids")] IReadOnlyList<long> Kids,
        [property: JsonPropertyName("score")] long Score,
        [property: JsonPropertyName("time")] long Time,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("url")] string Url
   );
}
