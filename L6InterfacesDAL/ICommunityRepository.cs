using L4DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6InterfacesDAL
{
	public interface ICommunityRepository
	{
		AllianceDto GetAllianceById(int id);
		List<AllianceDto> GetAllAlliancesByCommunityId(int communityId);
		List<AccountDto> GetAllAccountsByCommunityId(int communityId);
		List<AccountDto> GetAdminsById(int id);
		List<AccountDto> GetMembersById(int id);
		int GetHighestAllianceId();
		void CreateAlliance(AllianceDto allianceDto, int communityId);
		bool IsAccountInCommunity(AccountDto accountDto, CommunityDto communityDto);
		void AccountJoinsCommunity(AccountDto accountDto, CommunityDto communityDto, bool isAdmin);
		void AccountLeavesCommunity(AccountDto accountDto, CommunityDto communityDto);
	}
}
