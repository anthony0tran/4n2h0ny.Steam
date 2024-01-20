using _4n2h0ny.Steam.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace _4n2h0ny.Steam.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OwnerProfileController : ControllerBase
    {
        public OwnerProfileController()
        {
        }

        [HttpGet("{steamId}/commenters")]
        public IEnumerable<Profile> GetCommenters(string steamId)
        {
            return new List<Profile>();
        }
    }
}
