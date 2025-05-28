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

		public Community? Community { get; set; } = null;

		private CommunityRepository CommunityCollection = new CommunityRepository();

		public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        

		public void OnGet()
		{
			if (SelectedCommunityId.HasValue)
			{
				this.Community = CommunityCollection.FindCommunityByID(SelectedCommunityId.Value);

				this.Alliances = this.Community.GetAllAlliances();
				this.CommunitySubject = this.Community.subject;

				this.Accounts = this.Community.GetAllAccounts();
			}
		}
	}
}
