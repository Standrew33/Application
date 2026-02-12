using Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Client.Pages
{
    public class LoginModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [BindProperty]
        public LoginForm Form { get; set; } = new();
        public string? Error { get; set; }

        public void OnGet()
        {
            if (User?.Identity?.IsAuthenticated == true)
                Response.Redirect("/Profile");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("Api");
            var request = new LoginRequestData
            {
                Username = Form.Username.Trim(),
                Password = Form.Password,
            };

            using var response = await client.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                Error = "Incorrect login or password";
                return Page();
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseData>();
            if (loginResponse is null || string.IsNullOrWhiteSpace(loginResponse.AccessToken))
            {
                Error = "Failed to obtain token";
                return Page();
            }

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(loginResponse.ExpiresIn)
            };

            props.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = loginResponse.AccessToken } });

            //Set cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(
                    new List<Claim> { new(ClaimTypes.Name, request.Username) }, 
                    CookieAuthenticationDefaults.AuthenticationScheme)),
                props);

            return RedirectToPage("/Profile");
        }

        public class LoginForm
        {
            [Required(ErrorMessage = "Enter login")]
            [StringLength(100)]
            public string Username { get; set; } = default!;

            [Required(ErrorMessage = "Enter password")]
            [StringLength(100)]
            public string Password { get; set; } = default!;
        }
    }
}
