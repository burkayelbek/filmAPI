using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using DigiturkFilmAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiturkFilmAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<string> Login([FromQuery]LoginRequest request)
        {
            return _authenticationService.Login(request);
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult<User> Register(LoginRequest request)
        {
            return _authenticationService.Register(request);
        }

        [HttpGet]
        [Route("test")]
        [Authorize]
        public ActionResult<string> test()
        {
            return Ok();
        }
    }
}
