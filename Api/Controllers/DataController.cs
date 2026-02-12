using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                title = "Static data",
                user = User.Identity?.Name ?? "unknown",
                items = new[] { "Item #1", "Item #2", "Item #3" },
                data = DateTime.UtcNow
            });
        }
    }
}
