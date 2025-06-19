using L3LogicLayer;
using L5DAL;
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

		private CommunityCollection _communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());

		private Community? _community;

		public void OnPostChangeSubject()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());

			_community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
			_communityCollection.UpdateString(_community, "subject", Subject);
		}

		public void OnPostChangeLanguage()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());

			_community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
			_communityCollection.UpdateString(_community, "language", Language);
		}

		public void OnPostChangeDescription()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());

			_community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
			_communityCollection.UpdateString(_community, "description", Description);
		}

		public void OnPostChangeRules()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());

			_community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
			_communityCollection.UpdateString(_community, "rules", Rules);
		}

		public IActionResult OnPostDelete()
		{
			_communityCollection = new CommunityCollection(new CommunityCollectionRepository(), new CommunityRepository());

	        _community = _communityCollection.FindCommunityById(SelectedCommunityId, new CommunityRepository());
	        _communityCollection.DeleteCommunityById(_community);
			return RedirectToPage("Communities");
		}
	}
}
