using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4DTOs;

namespace L6InterfacesDAL
{
	public interface IAllianceRepository
	{
		List<AccountDto> GetCaptainsByAllianceId(int allianceId);
		List<AccountDto> GetCrewmembersByAllianceId(int allianceId);
		void ChangeSettings(AllianceDto allianceDto, int communityId);
		void DeleteById(int id);
		bool IsAccountInAlliance(AccountDto accountDto, AllianceDto allianceDto);
		void AccountJoinsAlliance(AccountDto accountDto, AllianceDto allianceDto, bool isCaptain);
		int GetCommunityIdByAlliance(AllianceDto allianceDto);
		void AccountLeavesAlliance(AccountDto accountDto, AllianceDto allianceDto);
	}
}
