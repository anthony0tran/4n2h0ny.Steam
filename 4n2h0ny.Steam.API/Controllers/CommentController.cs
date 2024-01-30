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
        public async Task CommentAllFriendCommenters(string comment, CancellationToken cancellationToken) => 
            await _commentService.CommentOnFriendCommenters(comment, cancellationToken);
    }
}
