using ClassLibraryDAL;
using ClassLibraryLogicLayer;
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
		public List<Community> Communities { get; set; } = new List<Community>();
		private Account currentAccount = null;

		private CommunityCollection CommunityCollection = new CommunityCollection(new CommunityRepository());
		private AccountCollection AccountCollection = new AccountCollection(new AccountRepository());

		public IActionResult OnPostCreate()
        {
	        this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);
			CommunityCollection.CreateCommunity(Subject, Language, Description, this.currentAccount);
			return RedirectToPage("Communities");
		}
	}
}
