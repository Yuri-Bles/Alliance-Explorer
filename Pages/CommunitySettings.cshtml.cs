using ClassLibraryDAL;
using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	[Authorize]
	public class CommunitySettingsModel : PageModel
	{
		[BindProperty]
		public string Subject { get; set; } = string.Empty;

		[BindProperty]
		public string Language { get; set; } = string.Empty;

		[BindProperty]
		public string Description { get; set; } = string.Empty;

		[BindProperty]
		public string Rules { get; set; } = string.Empty;

		[BindProperty(SupportsGet = true)]
		public int SelectedCommunityId { get; set; }

		private CommunityCollection communityCollection = new CommunityCollection(new CommunityRepository());

		private Community community;

		public void OnGet()
		{
			
		}

		public void OnPostChangeSubject()
		{
			community = communityCollection.FindCommunityByID(SelectedCommunityId);
			communityCollection.UpdateString(community, "subject", Subject);
		}

		public void OnPostChangeLanguage()
		{
			community = communityCollection.FindCommunityByID(SelectedCommunityId);
			communityCollection.UpdateString(community, "language", Language);
		}

		public void OnPostChangeDescription()
		{
			community = communityCollection.FindCommunityByID(SelectedCommunityId);
			communityCollection.UpdateString(community, "description", Description);
		}

		public void OnPostChangeRules()
		{
			community = communityCollection.FindCommunityByID(SelectedCommunityId);
			communityCollection.UpdateString(community, "rules", Rules);
		}

		public IActionResult OnPostDelete()
        {
	        community = communityCollection.FindCommunityByID(SelectedCommunityId);
	        communityCollection.DeleteCommunityByID(community);
			return RedirectToPage("Communities");
		}
	}
}
