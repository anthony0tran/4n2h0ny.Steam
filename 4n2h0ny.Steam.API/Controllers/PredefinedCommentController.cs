using _4n2h0ny.Steam.API.Context.Entities;
using _4n2h0ny.Steam.API.Models;
using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet("FirstPredefinedCommentInQueue")]
        [ProducesResponseType(typeof(PredefinedComment), 200)]
        public async Task<IActionResult> GetFirstPredefinedCommentInQueue(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _commentService.GetFirstPredefinedCommentInQueue(cancellationToken);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("List")]
        [ProducesResponseType(typeof(ICollection<PredefinedComment>), 200)]
        public async Task<IActionResult> ListPredefinedComments(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _commentService.ListPredefinedComments(cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
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

        [HttpPut("{predefinedCommentId}/priority")]
        public async Task<IActionResult> SetPriority(Guid predefinedCommentId, CommentPriority priority, CancellationToken cancellationToken)
        {
            try
            {
                await _commentService.SetPriority(predefinedCommentId, priority, cancellationToken);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Ok();
        }
    }
}
