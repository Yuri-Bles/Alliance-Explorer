using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class AccountCreationModel : PageModel
	{
		[BindProperty] public string? Name { get; set; } = string.Empty;
		[BindProperty] public string? Password { get; set; } = string.Empty;
		[BindProperty] public string? Email { get; set; }
		[BindProperty] public DateOnly Birthday { get; set; }
		[BindProperty] public double Latitude { get; set; }
		[BindProperty] public double Longitude { get; set; }
		[BindProperty] public int MaxDistance { get; set; }
		[BindProperty] public string? ErrorMessage { get; set; } = string.Empty;

		private AccountCollection? _accountCollection;

		public IActionResult OnGet()
		{
			Birthday = DateOnly.FromDateTime(DateTime.Today);

			return Page();
		}

		public IActionResult OnPostSignUp()
		{
			_accountCollection = new AccountCollection(new AccountCollectionRepository());

			this.ErrorMessage = _accountCollection.SignInCheck(Name, Password, Email, Birthday, Latitude, Longitude);
			if (this.ErrorMessage == null)
			{
				_accountCollection.CreateAccount(Name, Password, Email, Birthday, Latitude, Longitude, MaxDistance);
				return RedirectToPage("/index");
			}
			else
			{
				return Page();
			}
		}
	}
}
