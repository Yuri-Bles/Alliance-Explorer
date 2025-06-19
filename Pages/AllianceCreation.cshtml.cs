using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	[Authorize]
	public class AllianceCreationModel : PageModel
	{
		[BindProperty(SupportsGet = true)] public int? SelectedCommunityId { get; set; }
		[BindProperty] public string Name { get; set; } = string.Empty;
		[BindProperty] public string Language { get; set; } = string.Empty;
		[BindProperty] public string Rules { get; set; } = string.Empty;
		[BindProperty] public bool AgeChecked { get; set; } = false;
		[BindProperty] public bool AgeIsForced { get; set; } = false;
		[BindProperty] public int? MinimumAge { get; set; } = null;
		[BindProperty] public int? MaximumAge { get; set; } = null;
		[BindProperty] public bool Online { get; set; } = false;
		[BindProperty] public bool OnLocation { get; set; } = false;
		[BindProperty] public double? Latitude { get; set; } = null;
		[BindProperty] public double? Longitude { get; set; } = null;
		[BindProperty] public bool AllowCrewmemberEvents { get; set; } = false;

		[BindProperty] public string? ErrorMessage { get; set; }


		private CommunityCollection? _communityCollection;
		private AccountCollection? _accountCollection;

		public Community? Community { get; set; } = null;
		private Account? _currentAccount = null;

		public IActionResult OnPostCreate()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository());
			_accountCollection = new AccountCollection(new AccountCollectionRepository());


			if (SelectedCommunityId.HasValue)
			{
				this.Community = _communityCollection.FindCommunityById(SelectedCommunityId.Value);
				Alliance alliance = new Alliance(-1, this.Name, this.MinimumAge, this.MaximumAge, this.Language, this.Latitude, this.Longitude, this.Rules, this.AgeIsForced, this.OnLocation, this.Online, this.AllowCrewmemberEvents, new AllianceRepository());

				if (alliance.Name != "" && alliance.Language != "")
				{
					if (alliance.IsOnLocation || alliance.IsOnline)
					{
						if ((AgeChecked && MinimumAge <= MaximumAge) || !AgeChecked)
						{
							if (alliance is { IsOnLocation: true, Latitude: not null, Longitude: not null } || !alliance.IsOnLocation)
							{
								this._currentAccount = _accountCollection.GetAccountByName(User.Identity!.Name!);
								this.Community.CreateAlliance(this._currentAccount, alliance, this.AgeChecked);
								return RedirectToPage("Community", new { SelectedCommunityId = this.SelectedCommunityId });
							}
							else
							{
								ErrorMessage = "If you want to have in person selected, you need to specify where using latitude and longitude.";
								return Page();
							}
						}
						else
						{
							ErrorMessage = "Minimum age cannot be higher than maximum age.";
							return Page();
						}
					}
					else
					{
						ErrorMessage = "Please select either 'Online' or 'In person' or both.";
						return Page();
					}
				}
				else
				{
					ErrorMessage = "All required fields must be filled in.";
					return Page();
				}
			}
			else
			{
				ErrorMessage = "No community selected.";
				return Page();
			}
		}
	}
}
