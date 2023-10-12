using FluentValidation;
using MediatR;
using TheBestStories.Api.Mappers;
using TheBestStories.Api.RequestValidators;
using TheBestStories.Api.Responses;
using TheBestStories.Core.Models;

namespace TheBestStories.Api
{
    internal static class ServiceRegistration
    {
        internal static void RegisterApiServices(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddValidatorsFromAssemblyContaining<GetBestStoriesRequestValidator>();

            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddScoped<IObjectMapper<GetStoryDetailResponse, HackerNewsStoryDetailsResponse>, GetStoryDetailResponseMapper>();
        }
    }
}
