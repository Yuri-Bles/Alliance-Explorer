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

		[BindProperty(SupportsGet = true)]
		public bool IsMember { get; set; } = true;

		public Community? Community { get; set; } = null;

		private CommunityCollection? _communityCollection;
		private AccountCollection? _accountCollection;

		private Account? _currentAccount = null;

		public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Account?> Admins { get; set; } = new List<Account?>();
        public List<Account?> Members { get; set; } = new List<Account?>();


		public IActionResult OnGet()
		{
			try
			{
				_communityCollection = new CommunityCollection(new CommunityCollectionRepository());
				_accountCollection = new AccountCollection(new AccountCollectionRepository());
				this.Community = _communityCollection.FindCommunityById(SelectedCommunityId.Value);
				this.Admins = Community.GetAdmins();
				this.Members = Community.GetMembers();
			}
			catch
			{
				return RedirectToPage("/error");
			}

			foreach (var admin in Admins)
			{
				if (admin == _currentAccount)
				{
					this.IsMember = false;
					break;
				}
			}

			if (SelectedCommunityId.HasValue)
			{
				this.Alliances.Clear();
				this.Alliances = this.Community.GetAllAlliances(true);
				this.CommunitySubject = this.Community.Subject;

				this.Admins = this.Community.GetAdmins();
				this.Members = this.Community.GetMembers();

				this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);
				this.Joined = this.Community.IsAccountInCommunity(this._currentAccount);
			}

			return Page();
		}

		public IActionResult OnPostJoin()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository());
			_accountCollection = new AccountCollection(new AccountCollectionRepository());

			this.Community = _communityCollection.FindCommunityById(SelectedCommunityId.Value);
			this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);

			this.Community.AccountJoinsCommunity(_currentAccount, false);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId });
		}

		public IActionResult OnPostLeave()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository());
			_accountCollection = new AccountCollection(new AccountCollectionRepository());

			this.Community = _communityCollection.FindCommunityById(SelectedCommunityId.Value);
			this._currentAccount = _accountCollection.GetAccountByName(User.Identity.Name);

			this.Community.AccountLeavesCommunity(_currentAccount);

			return RedirectToPage(new { SelectedCommunityId = SelectedCommunityId });
		}
	}
}
