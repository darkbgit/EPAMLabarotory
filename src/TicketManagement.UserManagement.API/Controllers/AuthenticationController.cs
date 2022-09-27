using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.UserManagement.Services.Interfaces;

namespace TicketManagement.UserManagement.API.Controllers
{
    [Route("api/user-management/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IJwtService _jwtService;

        public AuthenticationController(IUsersService usersService,
            IJwtService jwtService)
        {
            _usersService = usersService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Get Jwt token if user with that name and password exists.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _usersService.LoginAsync(request);

            return Ok(result);
        }

        /// <summary>
        /// Validate token.
        /// </summary>
        [HttpPost("validate-token")]
        public ActionResult<bool> ValidateToken([FromBody] TokenRequest request)
        {
            var result = _jwtService.ValidateToken(request.Token);

            return result;
        }
    }
}
