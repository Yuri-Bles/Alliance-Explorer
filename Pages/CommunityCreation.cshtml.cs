using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class CommunityCreationModel : PageModel
	{
		[BindProperty]
		public string subject { get; set; } = string.Empty;

		[BindProperty]
		public string language { get; set; } = string.Empty;

		[BindProperty]
		public string description { get; set; } = string.Empty;
		public List<Community> Communities { get; set; } = new List<Community>();
        public void Onget()
        {
			
        }

        public void OnPostCreate()
        {
	        CommunityCollection communityCollection = new CommunityCollection();
			communityCollection.CreateCommunity(subject, language, description);
        }
	}
}
