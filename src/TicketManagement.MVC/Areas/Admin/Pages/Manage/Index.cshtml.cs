#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Refit;
using TicketManagement.Core.Public.Constants;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.MVC.Clients.UserManagement;
using TimeZoneNames;

namespace TicketManagement.MVC.Areas.Admin.Pages.Manage
{
    public class IndexModel : PageModel
    {
        private readonly IUsersClient _usersClient;
        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public IndexModel(IUsersClient usersClient,
            IOptions<RequestLocalizationOptions> locOptions)
        {
            _usersClient = usersClient;
            _locOptions = locOptions;
        }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<SelectListItem> TimeZones { get; set; }

        public IEnumerable<SelectListItem> Cultures { get; set; }

        private void Load(UserDto user)
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            Username = user.UserName;

            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Surname = user.Surname,
                TimeZoneId = user.TimeZoneId,
                Language = user.Language,
                Balance = user.Balance,
            };

            var cultureName = requestCulture.RequestCulture.Culture.Name == "be-BY" ? "en-US" : requestCulture.RequestCulture.Culture.Name;
            var timeZoneList = TimeZoneInfo
                .GetSystemTimeZones()
                .Select(t => new SelectListItem
                {
                    Text = TZNames.GetDisplayNameForTimeZone(t.Id, cultureName),
                    Value = t.Id,
                })
                .ToList();

            TimeZones = timeZoneList;

            var cultureItems = _locOptions.Value.SupportedUICultures
                ?.Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.DisplayName,
                })
                .ToList();

            Cultures = cultureItems;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UserDto user;

            try
            {
                user = await _usersClient.GetUser();
            }
            catch (ApiException)
            {
                return NotFound("Unable to load user.");
            }

            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserDto user;

            try
            {
                user = await _usersClient.GetUser();
            }
            catch (ApiException)
            {
                return NotFound("Unable to load user.");
            }

            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            var userForUpdateRequest = new UserForUpdateRequest
            {
                Id = user.Id,
                PhoneNumber = Input.PhoneNumber,
                FirstName = Input.FirstName,
                Surname = Input.Surname,
                Language = Input.Language,
                TimeZoneId = Input.TimeZoneId,
            };

            try
            {
                await _usersClient.UpdateUser(user.Id, userForUpdateRequest);
            }
            catch (ApiException)
            {
                StatusMessage = "Unexpected error when trying to update user.";
                return RedirectToPage();
            }

            var isLocaleChanged = user.Language != Input.Language;
            var isTimeZoneChanged = user.TimeZoneId != Input.TimeZoneId;

            if ((isLocaleChanged || isTimeZoneChanged) && HttpContext.User.Identity is ClaimsIdentity identity)
            {
                if (isLocaleChanged)
                {
                    var existClaim = identity.FindFirst(ClaimTypes.Locality);
                    if (existClaim != null)
                    {
                        identity.RemoveClaim(existClaim);
                    }

                    identity.AddClaim(new Claim(ClaimTypes.Locality, Input.Language));
                }

                if (isTimeZoneChanged)
                {
                    var existClaim = identity.FindFirst(CustomClaimTypes.TimeZoneId);
                    if (existClaim != null)
                    {
                        identity.RemoveClaim(existClaim);
                    }

                    identity.AddClaim(new Claim(CustomClaimTypes.TimeZoneId, Input.TimeZoneId));
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Time zone")]
            public string TimeZoneId { get; set; }

            [Display(Name = "Language")]
            public string Language { get; set; }

            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Display(Name = "Balance")]
            public decimal Balance { get; set; }
        }
    }
}
