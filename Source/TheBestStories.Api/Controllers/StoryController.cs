using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheBestStories.Api.Requests;
using TheBestStories.Api.Responses;

namespace TheBestStories.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class StoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoryController(IMediator mediator) => _mediator = mediator;

        [HttpGet("stories")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<GetStoryDetailResponse>>> GetBestStoriesAsync([FromQuery] int topNStories)
        {
            var response = await _mediator.Send(new GetBestStoriesRequest(topNStories));
            // test
            return Ok(response);
        }
    }
}
