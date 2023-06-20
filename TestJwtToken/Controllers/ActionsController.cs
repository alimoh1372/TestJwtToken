using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestJwtToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActionsController : ControllerBase
    {
        [Route("Sum")]
        [Authorize]
        [HttpGet]
        public IActionResult Sum(int number1, int number2)
        {
            return Ok(number1 + number2);
        }

    }
}
