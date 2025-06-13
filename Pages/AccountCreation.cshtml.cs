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
	public class AccountCreationModel : PageModel
	{
		[BindProperty] public string Name { get; set; } = string.Empty;
		[BindProperty] public string Password { get; set; } = string.Empty;
		[BindProperty] public string Email { get; set; }
		[BindProperty] public DateOnly Birthday { get; set; }
		[BindProperty] public double Latitude { get; set; }
		[BindProperty] public double Longitude { get; set; }
		[BindProperty] public int MaxDistance { get; set; }
		[BindProperty] public string? ErrorMessage { get; set; } = string.Empty;

		AccountCollection accountCollection = new AccountCollection(new AccountRepository());

		public void OnGet()
		{
			Birthday = DateOnly.FromDateTime(DateTime.Today);
		}

		public IActionResult OnPostSignUp()
		{
			this.ErrorMessage = accountCollection.SignInCheck(Name, Password, Email, Birthday, Latitude, Longitude);
			if (this.ErrorMessage == null)
			{
				accountCollection.CreateAccount(Name, Password, Email, Birthday, Latitude, Longitude, MaxDistance);
				return RedirectToPage("/index");
			}
			else
			{
				return Page();
			}
		}
	}
}
