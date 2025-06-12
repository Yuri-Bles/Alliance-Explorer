using System.Dynamic;
using ClassLibraryDAL;
using ClassLibraryLogicLayer;
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
		[BindProperty] public bool AgeChecked { get; set; } = false;
		[BindProperty] public bool AgeIsForced { get; set; } = false;
		[BindProperty] public int? MinimumAge { get; set; } = null;
		[BindProperty] public int? MaximumAge { get; set; } = null;
		[BindProperty] public bool Online { get; set; } = false;
		[BindProperty] public bool OnLocation { get; set; } = false;
		[BindProperty] public double? Latitude { get; set; } = null;
		[BindProperty] public double? Longitude { get; set; } = null;
		[BindProperty] public bool AllowCrewmemberEvents { get; set; } = false;

		[BindProperty] public string ErrorMessage { get; set; } = null;


		private CommunityCollection CommunityCollection = new CommunityCollection(new CommunityRepository());
		public Community? Community { get; set; } = null;
		public Alliance? Alliance { get; set; } = null;

		public void OnGet()
		{
			Community = CommunityCollection.FindCommunityByID(this.SelectedCommunityId!.Value);
			Alliance = Community.GetAllianceByID(this.SelectedAllianceId);

			Name = Alliance.name;
			Language = Alliance.language;
			Rules = Alliance.rules;
			AgeIsForced = Alliance.ageIsForced;
			MinimumAge = Alliance.minimumAge;
			MaximumAge = Alliance.maximumAge;
			Online = Alliance.isOnline;
			OnLocation = Alliance.isOnLocation;
			Latitude = Alliance.latitude;
			Longitude = Alliance.longitude;
			AllowCrewmemberEvents = Alliance.allowCrewmemberEvents;

			if (MinimumAge != null || MaximumAge != null)
			{
				AgeChecked = true;
			}
			else
			{
				AgeChecked = false;
			}
		}

		public IActionResult OnPostChange()
		{
			if (SelectedCommunityId.HasValue)
			{
				this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);
				Alliance alliance = new Alliance(this.SelectedAllianceId, this.Name, this.MinimumAge, this.MaximumAge, this.Language, this.Latitude, this.Longitude, this.Rules, this.AgeIsForced, this.OnLocation, this.Online, this.AllowCrewmemberEvents);

				if (alliance.name != null && alliance.language != null && alliance.ageIsForced != null && alliance.isOnline != null && alliance.isOnLocation != null && alliance.allowCrewmemberEvents != null)
				{
					if (alliance.isOnLocation || alliance.isOnline)
					{
						if (AgeChecked && MinimumAge <= MaximumAge)
						{
							if ((alliance.isOnLocation && alliance.latitude != null && alliance.longitude != null) || !alliance.isOnLocation)
							{
								this.Alliance.ChangeSettings(alliance, this.AgeChecked);
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
			Community = CommunityCollection.FindCommunityByID(this.SelectedCommunityId!.Value);
			Alliance = Community.GetAllianceByID(SelectedAllianceId);
			Alliance.Delete();
			return RedirectToPage("Community", new { SelectedCommunityId = this.SelectedCommunityId });
		}
	}
}
