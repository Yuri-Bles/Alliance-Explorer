using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	[Authorize]
	public class AllianceSettingsModel : PageModel
	{
		[BindProperty(SupportsGet = true)] public int? SelectedCommunityId { get; set; }
		[BindProperty(SupportsGet = true)] public int SelectedAllianceId { get; set; }
		[BindProperty] public string Name { get; set; } = string.Empty;
		[BindProperty] public string Language { get; set; } = string.Empty;
		[BindProperty] public string? Rules { get; set; } = string.Empty;
		[BindProperty] public bool AgeChecked { get; set; }
		[BindProperty] public bool AgeIsForced { get; set; }
		[BindProperty] public int? MinimumAge { get; set; }
		[BindProperty] public int? MaximumAge { get; set; }
		[BindProperty] public bool Online { get; set; }
		[BindProperty] public bool OnLocation { get; set; }
		[BindProperty] public double? Latitude { get; set; }
		[BindProperty] public double? Longitude { get; set; }
		[BindProperty] public bool AllowCrewmemberEvents { get; set; }

		[BindProperty] public string ErrorMessage { get; set; } = null;


		private CommunityCollection? _communityCollection;
		public Community? Community { get; set; } = null;
		public Alliance? Alliance { get; set; } = null;

		public IActionResult OnGet()
		{
			try
			{
				_communityCollection = new CommunityCollection(new CommunityRepository());
				Community = _communityCollection.FindCommunityById(this.SelectedCommunityId!.Value);
			}
			catch
			{
				return RedirectToPage("/Error");
			}

			Alliance = Community.GetAllianceById(this.SelectedAllianceId);

			if (Alliance == null)
			{
				return RedirectToPage("/Error");
			}

			Name = Alliance.Name;
			Language = Alliance.Language;
			Rules = Alliance.Rules;
			AgeIsForced = Alliance.AgeIsForced;
			MinimumAge = Alliance.MinimumAge;
			MaximumAge = Alliance.MaximumAge;
			Online = Alliance.IsOnline;
			OnLocation = Alliance.IsOnLocation;
			Latitude = Alliance.Latitude;
			Longitude = Alliance.Longitude;
			AllowCrewmemberEvents = Alliance.AllowCrewmemberEvents;

			if (MinimumAge != null || MaximumAge != null)
			{
				AgeChecked = true;
			}
			else
			{
				AgeChecked = false;
			}

			return Page();
		}

		public IActionResult OnPostChange()
		{
			_communityCollection = new CommunityCollection(new CommunityRepository());

			if (SelectedCommunityId.HasValue)
			{
				this.Community = _communityCollection.FindCommunityById(SelectedCommunityId.Value);
				Alliance alliance = new Alliance(this.SelectedAllianceId, this.Name, this.MinimumAge, this.MaximumAge, this.Language, this.Latitude, this.Longitude, this.Rules, this.AgeIsForced, this.OnLocation, this.Online, this.AllowCrewmemberEvents);

				if (alliance.Name != "" && alliance.Language != "")
				{
					if (alliance.IsOnLocation || alliance.IsOnline)
					{
						if (AgeChecked && MinimumAge <= MaximumAge)
						{
							if (alliance is { IsOnLocation: true, Latitude: not null, Longitude: not null } || !alliance.IsOnLocation)
							{
								this.Alliance!.ChangeSettings(alliance, this.AgeChecked);
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

		public IActionResult OnPostDelete()
		{
			_communityCollection = new CommunityCollection(new CommunityRepository());

			Community = _communityCollection.FindCommunityById(this.SelectedCommunityId!.Value);
			Alliance = Community.GetAllianceById(SelectedAllianceId);
			Alliance.Delete();
			return RedirectToPage("Community", new { SelectedCommunityId = this.SelectedCommunityId });
		}
	}
}
