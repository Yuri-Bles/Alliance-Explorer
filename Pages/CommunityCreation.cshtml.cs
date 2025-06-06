using ClassLibraryDAL;
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

		private CommunityCollection communityCollection = new CommunityCollection(new CommunityRepository());

		public IActionResult OnPostCreate()
        {
			communityCollection.CreateCommunity(Subject, Language, Description);
			return RedirectToPage("Communities");
		}
	}
}
