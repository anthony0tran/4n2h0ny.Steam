using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredefinedCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public PredefinedCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("List")]
        [ProducesResponseType(typeof(ICollection<PredefinedComment>), 200)]
        public async Task<IActionResult> ListPredefinedComments(CancellationToken cancellationToken)
        {
            var result = await _commentService.ListPredefinedComments(cancellationToken);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddPredefinedComment(string commentString, CancellationToken cancellationToken)
        {
            try
            {
                await _commentService.AddPredefinedComment(commentString, cancellationToken);
            }
            catch (Exception ex) 
            {
                return Problem(ex.Message);
            }

            return Ok();
        }
    }
}
