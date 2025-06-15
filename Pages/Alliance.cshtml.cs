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
	public class AllianceModel : PageModel
	{
		[BindProperty(SupportsGet = true)]
		public int? SelectedAllianceId { get; set; }

		[BindProperty(SupportsGet = true)]
		public int SelectedCommunityId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? Name { get; set; }

		[BindProperty(SupportsGet = true)]
		public bool Joined { get; set; } = false;

		private CommunityCollection communityCollection = new CommunityCollection(new CommunityRepository());
		private AccountCollection AccountCollection = new AccountCollection(new AccountRepository());

		private Account currentAccount = null;

		public Community? Community { get; set; } = null;
		public Alliance? Alliance { get; set; } = null;

        public List<Account> Captains { get; set; } = new List<Account>();
        public List<Account> Crewmembers { get; set; } = new List<Account>();


		public void OnGet()
		{
			if (SelectedAllianceId.HasValue)
			{
				this.Community = communityCollection.FindCommunityByID(this.SelectedCommunityId);

				this.Alliance = Community.GetAllianceByID(SelectedAllianceId.Value);

				this.Name = this.Alliance.name;

				this.Captains = this.Alliance.GetCaptains();
				this.Crewmembers = this.Alliance.GetCrewMembers();

				this.currentAccount = this.AccountCollection.GetAccountByName(User.Identity.Name);

				if ((!this.Alliance.IsAccountInAlliance(this.currentAccount) &&
				    !this.Alliance.IsAccountInCommunityThatAllianceIsIn(this.currentAccount)) || this.Alliance.IsAccountInAlliance(this.currentAccount))
				{
					this.Joined = true;
				}
				else
				{
					this.Joined = false;
				}
			}
		}

		public IActionResult OnPostJoin()
		{
			this.Community = communityCollection.FindCommunityByID(SelectedCommunityId);
			this.Alliance = this.Community.GetAllianceByID(SelectedAllianceId.Value);
			this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);

			this.Alliance.AccountJoinsAlliance(currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId, SelectedAllianceId = SelectedAllianceId });
		}

		public IActionResult OnPostLeave()
		{
			this.Community = communityCollection.FindCommunityByID(SelectedCommunityId);
			this.Alliance = this.Community.GetAllianceByID(SelectedAllianceId.Value);
			this.currentAccount = AccountCollection.GetAccountByName(User.Identity.Name);

			this.Alliance.AccountLeavesAlliance(currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId, SelectedAllianceId = SelectedAllianceId });
		}
	}
}
