using _4n2h0ny.Steam.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Firefox;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly FirefoxDriver _driver;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
            _driver = WebDriverSingleton.Instance.Driver;
        }

        [HttpPost("friends")]
        public async Task CommentAllFriendCommenters(string comment, CancellationToken cancellationToken)
        {
            await _commentService.CommentOnFriendCommenters(comment, cancellationToken);
            _driver.Dispose();
        }
    }
}
