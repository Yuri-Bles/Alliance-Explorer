using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class CommunityModel : PageModel
	{
		[BindProperty(SupportsGet = true)] 
		public int? SelectedCommunityId { get; set; }

		[BindProperty(SupportsGet = true)]
		public string? CommunitySubject { get; set; }

		public Community? community { get; set; } = null;

		private CommunityCollection communityCollection = new CommunityCollection();

		public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        

		public void OnGet()
		{
			if (SelectedCommunityId.HasValue)
			{
				this.community = communityCollection.FindCommunityByID(SelectedCommunityId.Value);

				this.Alliances = community.GetAllAlliances();
				this.CommunitySubject = community.subject;

				this.Accounts = community.GetAllAccounts();
			}
		}
	}
}
