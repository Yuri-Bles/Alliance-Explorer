using L3LogicLayer;
using L5DAL;
using L6InterfacesDAL;
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
		public bool DisableJoin { get; set; } = false;

		[BindProperty(SupportsGet = true)]
		public bool DisableLeave { get; set; } = false;

		[BindProperty(SupportsGet = true)]
		public bool IsCrewmember { get; set; } = true;

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
				_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());
				_accountCollection = new AccountCollection(new AccountCollectionRepository());
				this.Community = _communityCollection.FindCommunityById(this.SelectedCommunityId, new CommunityRepository());
			}
			catch
			{
				return RedirectToPage("/Error");
			}

			if (SelectedAllianceId.HasValue)
			{
				this.Alliance = Community.GetAllianceById(SelectedAllianceId.Value);

				this.Name = this.Alliance.Name;

				this.Captains = this.Alliance.GetCaptains(true);
				this.Crewmembers = this.Alliance.GetCrewMembers(true);

				this._currentAccount = this._accountCollection.GetAccountByName(User.Identity.Name);

				foreach (var captain in Captains)
				{
					if (captain == _currentAccount)
					{
						this.IsCrewmember = false;
						break;
					}
				}

				if (Alliance.IsAccountInAlliance(this._currentAccount))
				{
					this.DisableJoin = true;
					this.DisableLeave = false;
				}
				else if (Alliance.IsAccountInCommunityThatAllianceIsIn(this._currentAccount, new CommunityCollectionRepository(), new CommunityRepository()))
				{
					this.DisableJoin = false;
					this.DisableLeave = true;
				}
				else
				{
					this.DisableJoin = true;
					this.DisableLeave = true;
				}
			}

			return Page();
		}

		public IActionResult OnPostJoin()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());
			_accountCollection = new AccountCollection(new AccountCollectionRepository());

			this.Community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
			this.Alliance = this.Community.GetAllianceById(SelectedAllianceId.Value);
			this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);

			this.Alliance.AccountJoinsAlliance(_currentAccount, false, new CommunityCollectionRepository(), new CommunityRepository());

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId, SelectedAllianceId = SelectedAllianceId });
		}

		public IActionResult OnPostLeave()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());
			_accountCollection = new AccountCollection(new AccountCollectionRepository());

			this.Community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
			this.Alliance = this.Community.GetAllianceById(SelectedAllianceId.Value);
			this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);

			this.Alliance.AccountLeavesAlliance(_currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId, SelectedAllianceId = SelectedAllianceId });
		}
	}
}
