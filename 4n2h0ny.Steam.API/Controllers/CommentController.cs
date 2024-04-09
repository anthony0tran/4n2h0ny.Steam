using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpPost("friendsWithActiveCommentThread")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        public async Task<IActionResult> CommentOnFriendsWithActiveCommentThread(string comment, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var messageIsValid = _commentService.ValidateComment(comment);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            var commentedOnCount = await _commentService.CommentOnFriendsWithActiveCommentThread(comment, cancellationToken);

            watch.Stop();

            return Ok(new CommentResponse(commentedOnCount, watch.Elapsed));
        }

        [HttpPost("friends")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        public async Task<IActionResult> CommentAllFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var messageIsValid = _commentService.ValidateComment(comment);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            var commentedOnCount = await _commentService.CommentOnFriendCommenters(comment, cancellationToken);

            watch.Stop();

            return Ok(new CommentResponse(commentedOnCount, watch.Elapsed));
        }


        [HttpPost("Test")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        public async Task<IActionResult> PreviewComment(string URI, string comment, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var messageIsValid = _commentService.ValidateComment(comment);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            await _commentService.PreviewComment(URI, comment, cancellationToken);

            watch.Stop();

            return Ok(new CommentResponse(1, watch.Elapsed));
        }
    }
}
