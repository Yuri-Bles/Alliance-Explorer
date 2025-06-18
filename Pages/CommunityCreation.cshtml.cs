using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	[Authorize]
	public class CommunityCreationModel : PageModel
	{
		[BindProperty]
		public string Subject { get; set; } = string.Empty;

		[BindProperty]
		public string Language { get; set; } = string.Empty;

		[BindProperty]
		public string Description { get; set; } = string.Empty;

		[BindProperty] public string? ErrorMessage { get; set; } = string.Empty;
		private Account? _currentAccount;

		private CommunityCollection? _communityCollection;
		private AccountCollection? _accountCollection;

		public IActionResult OnPostCreate()
		{
			_communityCollection = new CommunityCollection(new CommunityRepository());
			_accountCollection = new AccountCollection(new AccountRepository());

	        this.ErrorMessage = _communityCollection.CreateCheck(this.Subject, this.Language, this.Description);

	        if (this.ErrorMessage == null)
	        {
				this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);
				_communityCollection.CreateCommunity(Subject, Language, Description, this._currentAccount, new CommunityDal());
				return RedirectToPage("Communities");
			}
	        else
	        {
		        return Page();
	        }
		}
	}
}
