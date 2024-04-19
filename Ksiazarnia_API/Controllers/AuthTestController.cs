using Ksiazarnia_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ksiazarnia_API.Controllers
{
    [Route("api/AuthTest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> GetSomething()
        {
            return "Authenticated";
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<string>> GetSomething(int someInt)
        {
            return "Authenticated as admin";
        }
    }
}
