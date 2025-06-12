using System.Security.Claims;
using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace Alliance_Explorer.Pages
{
	public class IndexModel : PageModel
	{
		[BindProperty] public string Name { get; set; } = string.Empty;
		[BindProperty] public string Password { get; set; } = string.Empty;

		AccountCollection accountCollection = new AccountCollection(new AccountRepository());

		public bool isLoggedIn = false;

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostLogIn()
		{
			if (accountCollection.LogInCheck(Name, Password))
			{
				// 1. Create user claims (info about the user)
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, Name)
				};

				// 2. Create identity and principal
				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);

				// 3. Sign in the user (create authentication cookie)
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

				// 4. Redirect to the protected page
				return RedirectToPage("Communities");
			}
			else
			{
				return Page();
			}
		}
	}
}
