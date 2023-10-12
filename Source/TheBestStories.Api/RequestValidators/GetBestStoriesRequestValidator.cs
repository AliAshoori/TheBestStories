using FluentValidation;
using TheBestStories.Api.Requests;

namespace TheBestStories.Api.RequestValidators
{
    public class GetBestStoriesRequestValidator : AbstractValidator<GetBestStoriesRequest>
    {
        public GetBestStoriesRequestValidator()
        {
            RuleFor(r => r.RequestedNumberOfStories)
                .Must(num => num >= Constants.RequestedNumberOfStoriesMin)
                .WithMessage(Strings.RequestedNumberOfStoriesErrorMessage);
        }
    }
}
