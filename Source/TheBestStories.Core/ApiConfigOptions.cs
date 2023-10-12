namespace TheBestStories.Core
{
    public class AppOptions
    {
        public const string AppConfigSection = "App";

        public HackerNewsApiOptions HackerNewsApi { get; set; }

        public MemoryCacheOptions MemoryCache { get; set; }

        public class HackerNewsApiOptions
        {
            public const string HackerNewsApi = "HackerNewsApi";

            public string BaseUrl { get; set; }

            public string Version { get; set; }

            public int RetryCount { get; set; }

            public int RetryWaitDurationInSec { get; set; }

            public int CircuitBreakDurationInSec { get; set; }

            public int HandledEventsAllowedBeforeBreaking { get; set; }
        }

        public class MemoryCacheOptions
        {
            public const string MemoryCache = "MemoryCache";

            public long DurationInSec { get; set; }

            public string BestStoriesKey { get; set; }
        }
    }
}
