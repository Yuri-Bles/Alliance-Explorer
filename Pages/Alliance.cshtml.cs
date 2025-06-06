using ClassLibraryDAL;
using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class AllianceModel : PageModel
	{
		[BindProperty(SupportsGet = true)]
		public int? SelectedAllianceId { get; set; }

		[BindProperty(SupportsGet = true)]
		public int SelectedCommunityId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? Name { get; set; }

		private CommunityCollection communityCollection = new CommunityCollection(new CommunityRepository());

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
			}
		}
	}
}
