using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4DTOs;
using L6InterfacesDAL;

namespace L7TestDAL
{
	public class TestAllianceRepository : IAllianceRepository
	{
		public List<AccountDto> Captains = new List<AccountDto>();
		public List<AccountDto> Crewmembers = new List<AccountDto>();

		public bool IsAccountInAlliance(AccountDto accountDto, AllianceDto allianceDto)
		{
			return false;
		}

		public List<AccountDto> GetCaptainsByAllianceId(int Id)
		{
			return Captains;
		}

		public List<AccountDto> GetCrewmembersByAllianceId(int Id)
		{
			return Crewmembers;
		}

		public void ChangeSettings(AllianceDto allianceDto, int communityId)
		{
			throw new NotImplementedException();
		}

		public void DeleteById(int id)
		{
			throw new NotImplementedException();
		}

		public void AccountJoinsAlliance(AccountDto accountDto, AllianceDto allianceDto, bool isCaptain)
		{
			if (isCaptain)
			{
				Captains.Add(accountDto);
			}
			else
			{
				Crewmembers.Add(accountDto);
			}

		}

		public void AccountLeavesAlliance(AccountDto accountDto, AllianceDto allianceDto)
		{
			throw new NotImplementedException();
		}

		public int GetCommunityIdByAlliance(AllianceDto allianceDto)
		{
			return 0;
		}
	}
}
