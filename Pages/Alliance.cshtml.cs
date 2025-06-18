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

		private CommunityCollection? _communityCollection;
		private AccountCollection? _accountCollection;

		private Account? _currentAccount;

		public Community? Community { get; set; } = null;
		public Alliance? Alliance { get; set; } = null;

        public List<Account?> Captains { get; set; } = new List<Account?>();
        public List<Account?> Crewmembers { get; set; } = new List<Account?>();


		public IActionResult OnGet()
		{
			try
			{
				_communityCollection = new CommunityCollection(new CommunityRepository());
				_accountCollection = new AccountCollection(new AccountRepository());
				this.Community = _communityCollection.FindCommunityById(this.SelectedCommunityId);
			}
			catch
			{
				return RedirectToPage("/Error");
			}

			if (SelectedAllianceId.HasValue)
			{
				this.Alliance = Community.GetAllianceById(SelectedAllianceId.Value);

				this.Name = this.Alliance.Name;

				this.Captains = this.Alliance.GetCaptains();
				this.Crewmembers = this.Alliance.GetCrewMembers();

				this._currentAccount = this._accountCollection.GetAccountByName(User.Identity.Name);

				if ((!this.Alliance.IsAccountInAlliance(this._currentAccount) &&
				    !this.Alliance.IsAccountInCommunityThatAllianceIsIn(this._currentAccount)) || this.Alliance.IsAccountInAlliance(this._currentAccount))
				{
					this.Joined = true;
				}
				else
				{
					this.Joined = false;
				}
			}

			return Page();
		}

		public IActionResult OnPostJoin()
		{
			_communityCollection = new CommunityCollection(new CommunityRepository());
			_accountCollection = new AccountCollection(new AccountRepository());

			this.Community = _communityCollection.FindCommunityById(SelectedCommunityId);
			this.Alliance = this.Community.GetAllianceById(SelectedAllianceId.Value);
			this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);

			this.Alliance.AccountJoinsAlliance(_currentAccount, false);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId, SelectedAllianceId = SelectedAllianceId });
		}

		public IActionResult OnPostLeave()
		{
			_communityCollection = new CommunityCollection(new CommunityRepository());
			_accountCollection = new AccountCollection(new AccountRepository());

			this.Community = _communityCollection.FindCommunityById(SelectedCommunityId);
			this.Alliance = this.Community.GetAllianceById(SelectedAllianceId.Value);
			this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);

			this.Alliance.AccountLeavesAlliance(_currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId, SelectedAllianceId = SelectedAllianceId });
		}
	}
}
