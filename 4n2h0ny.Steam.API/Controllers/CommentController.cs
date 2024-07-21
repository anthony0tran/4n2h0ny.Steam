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

        [HttpPost("predefined/friendsWithActiveCommentThread")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        public async Task<IActionResult> PredefinedCommentOnFriendsWithActiveCommentThread(CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var comment = await _commentService.GetFirstPredefinedCommentInQueue(cancellationToken);

            var messageIsValid = _commentService.ValidateComment(comment.CommentString);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            var commentedOnCount = await _commentService.CommentOnFriendsWithActiveCommentThread(comment.CommentString, cancellationToken);

            await _commentService.PredefinedCommentPostProcess(comment, cancellationToken);

            watch.Stop();

            return Ok(new CommentResponse(commentedOnCount, watch.Elapsed));
        }

        [HttpPost("predefined/friends")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        public async Task<IActionResult> PredefinedCommentAllFriendCommenters(CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var comment = await _commentService.GetFirstPredefinedCommentInQueue(cancellationToken);

            var messageIsValid = _commentService.ValidateComment(comment.CommentString);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            var commentedOnCount = await _commentService.CommentOnFriendCommenters(comment.CommentString, cancellationToken);

            await _commentService.PredefinedCommentPostProcess(comment, cancellationToken);

            watch.Stop();

            return Ok(new CommentResponse(commentedOnCount, watch.Elapsed));
        }

        [HttpPost("predefined/Test")]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        public async Task<IActionResult> PreviewPredefinedComment(string URI, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();

            var comment = await _commentService.GetFirstPredefinedCommentInQueue(cancellationToken);

            var messageIsValid = _commentService.ValidateComment(comment.CommentString);

            if (!messageIsValid)
            {
                return BadRequest("Message is invalid");
            }

            await _commentService.PreviewComment(URI, comment.CommentString, cancellationToken);

            watch.Stop();

            return Ok(new CommentResponse(1, watch.Elapsed));
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
