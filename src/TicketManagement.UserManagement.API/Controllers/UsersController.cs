using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;
using TicketManagement.Core.UserManagement.Services.Interfaces;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace TicketManagement.UserManagement.API.Controllers
{
    [Authorize]
    [Route("api/user-management/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger,
            IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("paginated-with-roles")]
        public async Task<ActionResult<PaginatedList<UserWithRolesDto>>> GetUsersForAdminList([FromQuery] PaginationRequest request)
        {
            const int defaultPage = 1;

            var users = await _usersService.GetPagedUsersWithRolesAsync(request.PageIndex ?? defaultPage, request.PageSize);

            return users;
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var id = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
            {
                return Unauthorized();
            }

            var user = await _usersService.GetUserByIdAsync(guidId);

            if (user == null)
            {
                return Unauthorized();
            }

            return user;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _usersService.GetUserByIdAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            return user;
        }

        [AllowAnonymous]
        [HttpPost("{id}/add-to-role")]
        public async Task<ActionResult> AddUserTorole([FromRoute] Guid id, [FromBody] AddToRoleRequest request)
        {
            if (!Enum.TryParse<Roles>(request.Role, out var role))
            {
                return BadRequest();
            }

            await _usersService.AddToRoleAsync(id, role);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("user-info")]
        public async Task<ActionResult<UserInfoResponse>> GetUserInfo([FromBody] TokenRequest request)
        {
            var userInfo = await _usersService.GetUserInfoAsync(request.Token);

            if (userInfo == null)
            {
                return BadRequest();
            }

            return userInfo;
        }

        /// <summary>
        /// Create user.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] UserForCreateDto dto)
        {
            var id = await _usersService.CreateUserAsync(dto);

            var user = await _usersService.GetUserByIdAsync(id);

            return CreatedAtAction(nameof(GetUserById), new { id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateRequest dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            await _usersService.UpdateUserAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _usersService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _usersService.DeleteUserAsync(id);

            return NoContent();
        }

        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetUserBalance([FromRoute] Guid id)
        {
            var user = await _usersService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersService.GetUserBalance(id);

            return Ok(result);
        }

        [HttpPatch("{id}/balance")]
        public async Task<IActionResult> TopUpUserBalance([FromRoute] Guid id, [FromBody] TopUpBalanceRequest request)
        {
            var user = await _usersService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _usersService.TopUpBalance(id, request.AdditionBalance);

            return Ok();
        }
    }
}
