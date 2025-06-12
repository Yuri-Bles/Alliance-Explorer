using ClassLibraryDAL;
using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	[Authorize]
	public class CommunitiesModel : PageModel
	{
        public List<Community> Communities { get; set; } = new List<Community>();
        public void Onget()
        {
		    CommunityCollection communityCollection = new CommunityCollection(new CommunityRepository());
		    Communities = communityCollection.GetAllCommunities();
        }
	}
}
