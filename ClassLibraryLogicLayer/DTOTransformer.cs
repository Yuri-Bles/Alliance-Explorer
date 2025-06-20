using L3LogicLayer;
using L4DTOs;
using L5DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3LogicLayer
{
	internal class DtoTransformer
	{
		public List<Account?> TurnAccountDtosIntoAccounts(List<AccountDto> accountDtOs)
		{
			List<Account?> accounts = new List<Account?>();
			foreach (var accountDto in accountDtOs)
			{
				Account? account = new Account(accountDto.Id, accountDto.Name, accountDto.Password, accountDto.Email,
					accountDto.Birthday, accountDto.Latitude, accountDto.Longitude, accountDto.MaxDistance);
				accounts.Add(account);
			}
			return accounts;
		}

		public List<AccountDto> TurnAccountsIntoAccountDtos(List<Account?> accounts)
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();
			foreach (var account in accounts)
			{
				AccountDto accountDto = new AccountDto();
				accountDto.Id = account.Id;
				accountDto.Name = account.Name;
				accountDto.Password = account.Password;
				accountDto.Email = account.Email;
				accountDto.Birthday = account.Birthday;
				accountDto.Latitude = account.Latitude;
				accountDto.Longitude = account.Longitude;
				accountDto.MaxDistance = account.MaxDistance;
				accountDtOs.Add(accountDto);
			}
			return accountDtOs;
		}

		public List<AllianceDto> TurnAlliancesIntoAllianceDtos(List<Alliance> alliances)
		{
			List<AllianceDto> allianceDtOs = new List<AllianceDto>();
			foreach (var alliance in alliances)
			{
				AllianceDto allianceDto = new AllianceDto();
				allianceDto.Id = alliance.GetId();
				allianceDto.Name = alliance.Name;
				allianceDto.MemberCount = alliance.MemberCount;
				allianceDto.MinimumAge = alliance.MinimumAge;
				allianceDto.MaximumAge = alliance.MaximumAge;
				allianceDto.Language = alliance.Language;
				allianceDto.Latitude = alliance.Latitude;
				allianceDto.Longitude = alliance.Longitude;
				allianceDto.Rules = alliance.Rules;
				allianceDto.AgeIsForced = alliance.AgeIsForced;
				allianceDto.IsOnline = alliance.IsOnline;
				allianceDto.IsOnLocation = alliance.IsOnLocation;
				allianceDto.AllowCrewmemberEvents = alliance.AllowCrewmemberEvents;

				allianceDtOs.Add(allianceDto);
			}
			return allianceDtOs;
		}

		public List<Alliance> TurnAllianceDtosIntoAlliances(List<AllianceDto> allianceDtOs)
		{
			List<Alliance> alliances = new List<Alliance>();
			foreach (var allianceDto in allianceDtOs)
			{
				Alliance alliance = new Alliance
				(
					allianceDto.Id, allianceDto.Name, allianceDto.MinimumAge, allianceDto.MaximumAge,
					allianceDto.Language, allianceDto.Latitude, allianceDto.Longitude, allianceDto.Rules,
					allianceDto.AgeIsForced, allianceDto.IsOnLocation, allianceDto.IsOnline,
					allianceDto.AllowCrewmemberEvents, new AllianceRepository()
				);
				alliances.Add(alliance);
			}
			return alliances;
		}

		public List<CommunityDto> TurnCommunitiesIntoCommunityDtos(List<Community> communities)
		{
			List<CommunityDto> communityDtOs = new List<CommunityDto>();
			foreach (var community in communities)
			{
				CommunityDto communityDto = new CommunityDto();
				communityDto.Id = community.Id;
				communityDto.Subject = community.Subject;
				communityDto.Description = community.Description;
				communityDto.Language = community.Language;
				communityDto.Rules = community.Rules;
				communityDto.Alliances = this.TurnAlliancesIntoAllianceDtos(community.GetAllAlliances(false));
				communityDto.MemberCount = community.MemberCount;

				communityDtOs.Add(communityDto);
			}
			return communityDtOs;
		}
	}
}
