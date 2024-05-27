using Icp.HotelAPI.Servicios.ValidateService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ValidateController
{
    //[ApiController]
    //[Route("api/validate")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public class TokenController : ControllerBase
    //{
    //    private readonly IValidateInterface validateService;

    //    public TokenController(IValidateInterface validateService)
    //    {
    //        this.validateService = validateService;
    //    }

    //    [HttpGet]
    //    [AllowAnonymous]
    //    public IActionResult ValidateToken(string token)
    //    {
    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return BadRequest(new { Message = "Es necesario tener un token" });
    //        }

    //        var isValid = validateService.ValidateToken(token);
    //        if (isValid)
    //        {
    //            return Ok(new { Message = "El token es válido" });
    //        }
    //        else
    //        {
    //            return Unauthorized(new { Message = "Token invalido" });
    //        }
    //    }
    //}

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
