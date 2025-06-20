using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4DTOs;
using L6InterfacesDAL;

namespace L7TestDAL
{
	public class TestCommunityRepository : ICommunityRepository
	{
		public List<AllianceDto> Alliances = new List<AllianceDto>();
		public List<AccountDto> Admins = new List<AccountDto>();
		public List<AccountDto> Members = new List<AccountDto>();

		public AllianceDto GetAllianceById(int id)
		{
			return new AllianceDto();
		}

		public List<AllianceDto> GetAllAlliancesByCommunityId(int communityId)
		{
			return this.Alliances;
		}

		public List<AccountDto> GetAllAccountsByCommunityId(int communityId)
		{
			return this.Admins;
		}

		public int GetHighestAllianceId()
		{
			return 0;
		}

		public void CreateAlliance(AllianceDto allianceDto, int communityId)
		{
			this.Alliances.Add(allianceDto);
		}

		public void ChangeAlliancePreferences(AllianceDto allianceDto, int communityId)
		{
			this.Alliances.Add(allianceDto);
		}

		public List<AccountDto> GetAdminsById(int id)
		{
			return this.Admins;
		}

		public List<AccountDto> GetMembersById(int id)
		{
			return this.Members;
		}

		public bool IsAccountInCommunity(AccountDto accountDto, CommunityDto communityDto)
		{
			return true;
		}

		public void AccountJoinsCommunity(AccountDto accountDto, CommunityDto communityDto, bool isAdmin)
		{
			if (isAdmin)
			{
				this.Admins.Add(accountDto);
			}
			else
			{
				this.Members.Add(accountDto);
			}
		}

		public void AccountLeavesCommunity(AccountDto accountDto, CommunityDto communityDto)
		{
			foreach (var admin in Admins)
			{
				if (admin == accountDto)
				{
					Admins.Remove(admin);
					break;
				}
			}

			foreach (var member in Members)
			{
				if (member == accountDto)
				{
					Members.Remove(accountDto);
					break;
				}
			}
		}
	}
}
