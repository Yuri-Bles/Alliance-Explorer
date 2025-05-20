using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class CommunityCreationModel : PageModel
	{
        public List<Community> Communities { get; set; } = new List<Community>();
        public void Onget()
        {
	        bool useDatabase = false;

	        if (useDatabase)
	        {
		        CommunityCollection communityCollectionDal = new CommunityCollection();
		        Communities = communityCollectionDal.GetAllCommunities();
			}
	        else
	        {
		        Communities.Add(new Community("Subject 1", "Description 1"));
		        Communities.Add(new Community("Subject 2", "Description 2"));
		        Communities.Add(new Community("Subject 3", "Description 3"));
		        Communities.Add(new Community("Subject 4", "Description 4"));
		        Communities.Add(new Community("Subject 5", "Description 5"));
		        Communities.Add(new Community("Subject 6", "Description 6"));
			}
        }
	}
}
