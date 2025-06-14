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

		private CommunityCollection CommunityCollection = new CommunityCollection(new CommunityRepository());
		private AccountCollection AccountCollection = new AccountCollection(new AccountRepository());

		private Account currentAccount = null;

		public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Account> Admins { get; set; } = new List<Account>();
        public List<Account> Members { get; set; } = new List<Account>();


		public void OnGet()
		{
			if (SelectedCommunityId.HasValue)
			{
				this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);

				this.Alliances = this.Community.GetAllAlliances();
				this.CommunitySubject = this.Community.subject;

				this.Admins = this.Community.GetAdmins();
				this.Members = this.Community.GetMembers();

				this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);
				this.Joined = this.Community.IsAccountInCommunity(this.currentAccount);
			}
		}

		public IActionResult OnPostJoin()
		{
			this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);
			this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);

			this.Community.AccountJoinsCommunity(currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId });
		}
	}
}
