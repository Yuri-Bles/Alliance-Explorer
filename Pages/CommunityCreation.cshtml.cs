using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class CommunityCreationModel : PageModel
	{
		[BindProperty]
		public string Subject { get; set; } = string.Empty;

		[BindProperty]
		public string Language { get; set; } = string.Empty;

		[BindProperty]
		public string Description { get; set; } = string.Empty;
		public List<Community> Communities { get; set; } = new List<Community>();

        public IActionResult OnPostCreate()
        {
	        CommunityRepository communityCollection = new CommunityRepository();
			communityCollection.CreateCommunity(false, Subject, Language, Description);
			return RedirectToPage("Communities");
		}
	}
}
