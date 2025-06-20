using L4DTOs;
using L5DAL;
using L6InterfacesDAL;

namespace L3LogicLayer
{
	public class Community
	{
		private readonly DtoTransformer _dtoTransformer = new DtoTransformer();
		private readonly ICommunityRepository _communityRepository;
		public int Id { get; private set; }
		public string Subject { get; private set; }
		public string? Description { get; private set; }
        public int MemberCount { get; private set; } = 0;
        public string? Language { get; private set; } = "English";
		public string? Rules { get; private set; } = "";

		private List<Alliance> _alliances = new List<Alliance>();
		private List<Account?> _admins = new List<Account?>();
		private List<Account?> _members = new List<Account?>();

		public Community(ICommunityRepository communityRepository, string subject, string language, string description)
		{
			this._communityRepository = communityRepository;
			this.Subject = subject;
			this.Language = language;
			this.Description = description;
		}

		public Community(ICommunityRepository communityRepository, string subject, string language, string description, int id, int memberCount, string rules)
		{
			this._communityRepository = communityRepository;
			this.Subject = subject;
			this.Language = language;
			this.Description = description;

			this.Id = id;
			this.MemberCount = memberCount;
			this.Rules = rules;
		}

		public void SetId(int id)
		{
			this.Id = id;
		}

		public Alliance GetAllianceById(int id)
		{
			AllianceDto allianceDto = _communityRepository.GetAllianceById(id);
			List<AllianceDto> allianceDtoList = new List<AllianceDto>();
			allianceDtoList.Add(allianceDto);
			List<Alliance> allianceList = _dtoTransformer.TurnAllianceDtosIntoAlliances(allianceDtoList);
			Alliance alliance = allianceList[0];
			return alliance;
		}

		public List<Alliance> GetAllAlliances(bool refreshList)
		{
			if (refreshList)
			{
				this._alliances.Clear();

				List<AllianceDto> allianceDtOs = _communityRepository.GetAllAlliancesByCommunityId(this.Id);

				foreach (var allianceDto in allianceDtOs)
				{
					Alliance alliance = new Alliance(allianceDto.Id, allianceDto.Name, allianceDto.MinimumAge,
						allianceDto.MaximumAge, allianceDto.Language, allianceDto.Latitude, allianceDto.Longitude,
						allianceDto.Rules, allianceDto.AgeIsForced, allianceDto.IsOnLocation, allianceDto.IsOnline,
						allianceDto.AllowCrewmemberEvents, new AllianceRepository());
					this._alliances.Add(alliance);
				}
			}

			return this._alliances;
		}

		public List<Account?> GetAdmins()
		{
			this._admins.Clear();
			List<AccountDto> accountDtOs = _communityRepository.GetAdminsById(this.Id);
			this._admins = _dtoTransformer.TurnAccountDtosIntoAccounts(accountDtOs);
			return this._admins;
		}

		public List<Account?> GetMembers()
		{
			this._members.Clear();
			List<AccountDto> accountDtOs = _communityRepository.GetMembersById(this.Id);
			this._members = _dtoTransformer.TurnAccountDtosIntoAccounts(accountDtOs);
			return this._members;
		}

		public void CreateAlliance(Account? account, Alliance alliance, bool ageChecked)
		{
			//Check to see if all non-nullables aren't null
			if (alliance is { Name: not null, Language: not null } && alliance.AgeIsForced != null && alliance.IsOnline != null && alliance.IsOnLocation != null && alliance.AllowCrewmemberEvents != null)
			{
				//Check to see if at least one of them is true
				//(It is allowed for both to be true)
				if (alliance.IsOnLocation || alliance.IsOnline)
				{
					//Check to see if age is valid
					if ((ageChecked && alliance.MinimumAge <= alliance.MaximumAge) || !ageChecked)
					{
						//Check to see if latitude and longitude are filled in if in person is checked
						if (alliance is { IsOnLocation: true, Latitude: not null, Longitude: not null } || !alliance.IsOnLocation)
						{
							//Replaces placeholder with actual value
							if (alliance.GetId() == -1)
							{
								alliance.SetId(this._communityRepository.GetHighestAllianceId() + 1);
							}

							this._alliances.Add(alliance);
							List<Alliance> allianceList = new List<Alliance>();
							allianceList.Add(alliance);
							List<AllianceDto> allianceDtoList = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList);
							_communityRepository.CreateAlliance(allianceDtoList[0], this.Id);

							alliance.AccountJoinsAlliance(account, true, new CommunityCollectionRepository(), new CommunityRepository());
						}
					}
				}
			}
		}

		public bool IsAccountInCommunity(Account? account)
		{
			List<Account?> accountList = new List<Account?>();
			accountList.Add(account);
			AccountDto accountDto = _dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

			List<Community> communityList = new List<Community>();
			communityList.Add(this);
			CommunityDto communityDto = _dtoTransformer.TurnCommunitiesIntoCommunityDtos(communityList)[0];

			return _communityRepository.IsAccountInCommunity(accountDto, communityDto);
		}

		public void AccountJoinsCommunity(Account? account, bool isAdmin)
		{
			if (!IsAccountInCommunity(account))
			{
				List<Account?> accountList = new List<Account?>();
				accountList.Add(account);
				AccountDto accountDto = this._dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

				List<Community> communityList = new List<Community>();
				communityList.Add(this);
				CommunityDto communityDto = _dtoTransformer.TurnCommunitiesIntoCommunityDtos(communityList)[0];

				_communityRepository.AccountJoinsCommunity(accountDto, communityDto, isAdmin);
			}
		}

		public void AccountLeavesCommunity(Account? account)
		{
			if (IsAccountInCommunity(account))
			{
				List<Account?> accountList = new List<Account?>();
				accountList.Add(account);
				AccountDto accountDto = this._dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

				this._alliances = GetAllAlliances(true);

				foreach (var alliance in _alliances)
				{
					if (alliance.IsAccountInAlliance(account))
					{
						alliance.AccountLeavesAlliance(account);
					}
				}

				List<Community> communityList = new List<Community>();
				communityList.Add(this);
				CommunityDto communityDto = _dtoTransformer.TurnCommunitiesIntoCommunityDtos(communityList)[0];

				_communityRepository.AccountLeavesCommunity(accountDto, communityDto);
			}
		}
	}
}
