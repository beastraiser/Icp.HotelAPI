using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ValidateController
{
    [ApiController]
    [Route("api/validate")]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult ValidateToken()
        {
            return Ok(new { message = "El token es válido" });
        }
    }


}
