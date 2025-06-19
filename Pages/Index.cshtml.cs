using System.Security.Claims;
using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class IndexModel : PageModel
	{
		[BindProperty] public string? Name { get; set; } = string.Empty;
		[BindProperty] public string? Password { get; set; } = string.Empty;
		[BindProperty] public string? ErrorMessage { get; set; } = string.Empty;

		AccountCollection? _accountCollection;

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity!.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Index");
            }

            try
            {
	            _accountCollection = new AccountCollection(new AccountCollectionRepository());
            }
            catch
            {
	            return RedirectToPage("/Error");
            }

			return Page();
        }

        public async Task<IActionResult> OnPostLogIn()
		{
			_accountCollection = new AccountCollection(new AccountCollectionRepository());

			if (_accountCollection.LogInCheck(Name, Password))
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
				this.ErrorMessage = "Name or Password is incorrect, please try again.";
				return Page();
			}
		}

		public IActionResult OnPostSignUp()
		{
			return RedirectToPage("/AccountCreation");
		}
	}
}
