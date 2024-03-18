using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("friends")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CommentAllFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            var messageIsValid = _commentService.ValidateComment(comment);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            var commentedOnCount = await _commentService.CommentOnFriendCommenters(comment, cancellationToken);
            return Ok(commentedOnCount);
        }


        [HttpPost("Test")]
        public async Task PreviewComment(string URI, string comment, CancellationToken cancellationToken) =>
            await _commentService.PreviewComment(URI, comment, cancellationToken);
    }
}
