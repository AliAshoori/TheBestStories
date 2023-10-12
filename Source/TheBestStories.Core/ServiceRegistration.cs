using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheBestStories.Core.Interfaces;
using TheBestStories.Core.Services;

namespace TheBestStories.Core
{
    public static class ServiceRegistration
    {
        public static void RegisterCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddScoped<IHackerNewsCacheService, HackerNewsCacheService>();
            services.AddScoped<IHackerNewsStoryService, HackerNewsStoryService>();

            services.AddScoped<IHackerNewsApiClientPolicyService, HackerNewsApiClientPolicyService>();
            services.AddScoped<IHackerNewsApiClientPolicyExecutorService, HackerNewsApiClientPolicyExecutorService>();

            services.AddHttpClient<IHackerNewsApiClientService, HackerNewsApiClientService>(httpClient =>
            {
                AppOptions options = new();
                configuration.GetSection(AppOptions.AppConfigSection).Bind(options);

                httpClient.BaseAddress = new Uri($"{options.HackerNewsApi.BaseUrl}/{options.HackerNewsApi.Version}/");
            });
        }
    }
}
