using CarbonaraWebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarbonaraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Ping()
        {
            return Ok();
        }

    }
}
