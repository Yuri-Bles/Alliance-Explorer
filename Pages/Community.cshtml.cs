using ClassLibraryLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alliance_Explorer.Pages
{
	public class CommunityModel : PageModel
	{
        public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public List<Account> Accounts { get; set; } = new List<Account>();
        public void Onget()
        {
	        
        }
	}
}
