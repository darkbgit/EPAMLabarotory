#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;
using TicketManagement.Core.Public.Constants;
using TicketManagement.Core.Public.Extensions;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;
using TicketManagement.MVC.Clients.UserManagement;

namespace TicketManagement.MVC.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUsersClient _usersClient;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger,
            IUsersClient usersClient)
        {
            _logger = logger;
            _usersClient = usersClient;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            const string DefaultTimeZone = "UTC";

            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginRequest = new LoginRequest
            {
                UsernameOrEmail = Input.EmailOrName,
                Password = Input.Password,
            };

            string token;

            try
            {
                token = await _usersClient.Login(loginRequest);
            }
            catch (ApiException e)
            {
                _logger.LogError("{e}", e);
                e.ErrorsToModelStateErrors(this);
                return Page();
            }

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            UserInfoResponse userInfo;

            var tokenRequest = new TokenRequest
            {
                Token = token,
            };

            try
            {
                userInfo = await _usersClient.GetUserInfo(tokenRequest);
            }
            catch (ApiException e)
            {
                _logger.LogError("{e}", e);
                e.ErrorsToModelStateErrors(this);
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Authentication, token),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                new Claim(ClaimTypes.Name, loginRequest.UsernameOrEmail),
                new Claim(ClaimTypes.Locality, userInfo.Locale ?? Cultures.English),
                new Claim(CustomClaimTypes.TimeZoneId, userInfo.TimeZoneId ?? DefaultTimeZone),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            foreach (var userInfoRole in userInfo.Roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userInfoRole));
            }

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("User {Email} logged in at {Time}.",
                loginRequest.UsernameOrEmail, DateTime.UtcNow);

            return RedirectToPage(returnUrl);
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Email or name")]
            public string EmailOrName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}
