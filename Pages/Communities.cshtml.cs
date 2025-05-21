using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class CommunitiesModel : PageModel
	{
        public List<Community> Communities { get; set; } = new List<Community>();
        public void Onget()
        {
	        bool useDatabase = true;

	        if (useDatabase)
	        {
		        CommunityCollection communityCollection = new CommunityCollection();
		        Communities = communityCollection.GetAllCommunities();
			}
	        else
	        {
		        Communities.Add(new Community("Subject 1", "Language", "Description 1"));
		        Communities.Add(new Community("Subject 2", "Language", "Description 2"));
		        Communities.Add(new Community("Subject 3", "Language", "Description 3"));
		        Communities.Add(new Community("Subject 4", "Language", "Description 4"));
		        Communities.Add(new Community("Subject 5", "Language", "Description 5"));
		        Communities.Add(new Community("Subject 6", "Language", "Description 6"));
			}
        }
	}
}
