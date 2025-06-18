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
	public class CommunityModel : PageModel
	{
		[BindProperty(SupportsGet = true)]
		public int? SelectedCommunityId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? CommunitySubject { get; set; }

		[BindProperty(SupportsGet = true)]
		public bool Joined { get; set; } = false;

		public Community? Community { get; set; } = null;

		private CommunityCollection CommunityCollection;
		private AccountCollection AccountCollection;

		private Account currentAccount = null;

		public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Account> Admins { get; set; } = new List<Account>();
        public List<Account> Members { get; set; } = new List<Account>();


		public IActionResult OnGet()
		{
			try
			{
				CommunityCollection = new CommunityCollection(new CommunityRepository());
				AccountCollection = new AccountCollection(new AccountRepository());
				this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);
			}
			catch
			{
				return RedirectToPage("/error");
			}

			if (SelectedCommunityId.HasValue)
			{
				this.Alliances.Clear();
				this.Alliances = this.Community.GetAllAlliances(true);
				this.CommunitySubject = this.Community.subject;

				this.Admins = this.Community.GetAdmins();
				this.Members = this.Community.GetMembers();

				this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);
				this.Joined = this.Community.IsAccountInCommunity(this.currentAccount);
			}

			return Page();
		}

		public IActionResult OnPostJoin()
		{
			CommunityCollection = new CommunityCollection(new CommunityRepository());
			AccountCollection = new AccountCollection(new AccountRepository());

			this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);
			this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);

			this.Community.AccountJoinsCommunity(currentAccount, false);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId });
		}

		public IActionResult OnPostLeave()
		{
			CommunityCollection = new CommunityCollection(new CommunityRepository());
			AccountCollection = new AccountCollection(new AccountRepository());

			this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);
			this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);

			this.Community.AccountLeavesCommunity(currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId });
		}
	}
}
