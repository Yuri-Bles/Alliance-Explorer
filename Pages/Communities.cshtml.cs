using L3LogicLayer;
using L5DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	[Authorize]
	public class CommunitiesModel : PageModel
	{
        public List<Community?> Communities { get; set; } = new List<Community?>();
        private CommunityCollection? _communityCollection;

        public IActionResult Onget()
        {
	        try
	        {
		        _communityCollection = new CommunityCollection(new CommunityRepository());
	        }
	        catch
	        {
		        return RedirectToPage("/Error");
	        }

		    Communities = _communityCollection.GetAllCommunities();

		    return Page();
        }
	}
}
