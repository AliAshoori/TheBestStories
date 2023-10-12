namespace TheBestStories.Core.Interfaces
{
    public interface IHackerNewsApiClientPolicyExecutorService
    {
        Task<HttpResponseMessage> ExecuteAsync(Func<Task<HttpResponseMessage>> apiCallAsyncAction);
    }
}
