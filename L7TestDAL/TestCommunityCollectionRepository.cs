using L4DTOs;
using L6InterfacesDAL;

namespace L7TestDAL
{
	public class TestCommunityCollectionRepository : ICommunityCollectionRepository
	{
		public List<CommunityDto> Communities = new List<CommunityDto>();

		public List<CommunityDto> GetAllCommunities()
		{

			for (int i = 0; i < 8; i++)
			{
				CommunityDto community = new CommunityDto();
				community.Id = i;
				community.Subject = $"Subject {i + 1}";
				community.Language = "English";
				community.Description = "Description of the community";
				community.Rules = "Rules of the community";
				Communities.Add(community);
			}

			return Communities;
		}

		public CommunityDto GetCommunityById(int id)
		{
			CommunityDto community = new CommunityDto();
			community.Id = 0;
			community.Subject = "Subject 1";
			community.Language = "English";
			community.Description = "Description of the community";
			community.Rules = null;
			return community;
		}

		public int GetMemberCount(CommunityDto communityDto)
		{
			return 1;
		}

		public int GetHighestId()
		{
			return 0;
		}

		public void CreateCommunity(CommunityDto communityDto)
		{

		}

		public void DeleteCommunityById(int id)
		{

		}

		public void UpdateString(int communityId, string updateValue, string newValue)
		{

		}
	}
}
