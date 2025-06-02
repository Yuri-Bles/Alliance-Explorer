using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
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

		private CommunityRepository communityRepository = new CommunityRepository();

		private Community community;

		public void OnGet()
		{
			
		}

		public void OnPostChangeSubject()
		{
			community = communityRepository.FindCommunityByID(SelectedCommunityId);
			communityRepository.UpdateString(community, "subject", Subject);
		}

		public void OnPostChangeLanguage()
		{
			community = communityRepository.FindCommunityByID(SelectedCommunityId);
			communityRepository.UpdateString(community, "language", Language);
		}

		public void OnPostChangeDescription()
		{
			community = communityRepository.FindCommunityByID(SelectedCommunityId);
			communityRepository.UpdateString(community, "description", Description);
		}

		public void OnPostChangeRules()
		{
			community = communityRepository.FindCommunityByID(SelectedCommunityId);
			communityRepository.UpdateString(community, "rules", Rules);
		}

		public IActionResult OnPostDelete()
        {
	        community = communityRepository.FindCommunityByID(SelectedCommunityId);
			communityRepository.DeleteCommunityByID(community);
			return RedirectToPage("Communities");
		}
	}
}
