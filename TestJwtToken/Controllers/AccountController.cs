using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestJwtToken.Models.Utility;
using TestJwtToken.Models.ViewModel;

namespace TestJwtToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAuthHelper _authHelper;

        public AccountController(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }


        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginFieldRequest request)
        {
            var result = _authHelper.Authenticate(request);

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
    }
}
