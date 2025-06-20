using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4DTOs;

namespace L6InterfacesDAL
{
	public interface ICommunityCollectionRepository
	{
		List<CommunityDto> GetAllCommunities();
		CommunityDto GetCommunityById(int id);
		int GetMemberCount(CommunityDto communityDto);
		int GetHighestId();
		void CreateCommunity(CommunityDto communityDto);
		void DeleteCommunityById(int communityId);
		void UpdateString(int communityId, string updateValue, string newValue);
	}

}
