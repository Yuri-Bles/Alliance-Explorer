using L4DTOs;
using L5DAL;
using L6InterfacesDAL;

namespace L3LogicLayer
{
	public class Alliance
	{
		private readonly IAllianceRepository _allianceRepository;
		private readonly DtoTransformer _dtoTransformer = new DtoTransformer();

		private int _id;
		public string Name { get; private set; }
		public int MemberCount { get; private set; }
		public int? MinimumAge { get; private set; }
		public int? MaximumAge { get; private set; }
		public string Language { get; private set; }
		public double? Latitude { get; private set; }
		public double? Longitude { get; private set; }
		public string? Rules { get; private set; }
		public bool AgeIsForced { get; private set; }
		public bool IsOnLocation { get; private set; }
		public bool IsOnline { get; private set; }
		public bool AllowCrewmemberEvents { get; private set; }

		private List<Account?> _captains = new List<Account?>();
		private List<Account?> _crewMembers = new List<Account?>();

		public Alliance(int id, string name, int? minimumAge, int? maximumAge, string language, double? latitude,
			double? longitude, string rules, bool ageIsForced, bool isOnLocation, bool isOnline,
			bool allowCrewmemberEvents, IAllianceRepository allianceRepository)
		{
			this._id = id;
			this.Name = name;
			this.MinimumAge = minimumAge;
			this.MaximumAge = maximumAge;
			this.Language = language;
			this.Rules = rules;
			this.AgeIsForced = ageIsForced;
			this.IsOnLocation = isOnLocation;
			this.IsOnline = isOnline;
			this.AllowCrewmemberEvents = allowCrewmemberEvents;
			this._allianceRepository = allianceRepository;

			this.Latitude = latitude is >= -90 and <= 90 ? latitude : 0;
			this.Longitude = longitude is >= -180 and <= 180 ? longitude : 0;

			SetAccountsFromDatabase();
		}

		private void SetAccountsFromDatabase()
		{
			this._captains = GetCaptains(true);
			this._crewMembers = GetCrewMembers(true);
		}

		public bool IsAccountInCommunityThatAllianceIsIn(Account? account, ICommunityCollectionRepository communityCollectionRepository, ICommunityRepository communityRepository)
		{
			List<Account?> accountList = new List<Account?>();
			accountList.Add(account);
			AccountDto accountDto = _dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

			List<Alliance> allianceList = new List<Alliance>();
			allianceList.Add(this);
			AllianceDto allianceDto = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList)[0];

			int communityId = _allianceRepository.GetCommunityIdByAlliance(allianceDto);
			CommunityCollection communityCollection = new CommunityCollection(communityCollectionRepository, communityRepository);
			Community? community = communityCollection.FindCommunityById(communityId, communityRepository);

			if (community.IsAccountInCommunity(account))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public int GetId()
		{
			return this._id;
		}

		public void SetId(int id)
		{
			this._id = id;
		}

		public List<Account?> GetCaptains(bool refresh)
		{
			if (refresh)
			{
				this._captains.Clear();
				List<AccountDto> captainDtOs = this._allianceRepository.GetCaptainsByAllianceId(this._id);
				this._captains = _dtoTransformer.TurnAccountDtosIntoAccounts(captainDtOs);
			}
			return this._captains;
		}

		public List<Account?> GetCrewMembers(bool refresh)
		{
			if (refresh)
			{
				this._crewMembers.Clear();
				List<AccountDto> crewmemberDtOs = this._allianceRepository.GetCrewmembersByAllianceId(this._id);
				this._crewMembers = _dtoTransformer.TurnAccountDtosIntoAccounts(crewmemberDtOs);
			}
			return this._crewMembers;
		}

		public int GetMemberCount(bool refresh)
		{
			this.GetCaptains(refresh);
			this.GetCrewMembers(refresh);
			return this._crewMembers.Count + this._captains.Count;
		}

		public void ChangeSettings(Alliance alliance, bool ageChecked)
		{
			//Check to see if all non-nullables aren't null
			if (alliance is { Name: not null, Language: not null })
			{
				//Check to see if at least one of them is true
				//(It is allowed for both to be true)
				if (alliance.IsOnLocation || alliance.IsOnline)
				{
					//Check to see if age is valid
					if (ageChecked && alliance.MinimumAge <= alliance.MaximumAge)
					{
						//Check to see if latitude and longitude are filled in if in person is checked
						if (alliance is { IsOnLocation: true, Latitude: not null, Longitude: not null } || !alliance.IsOnLocation)
						{
							List<Alliance> allianceList = new List<Alliance>();
							allianceList.Add(alliance);
							List<AllianceDto> allianceDtoList = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList);
							_allianceRepository.ChangeSettings(allianceDtoList[0], this._id);
						}
					}
				}
			}
		}

		public void Delete()
		{
			foreach (var captain in _captains)
			{
				List<Account?> captainList = new List<Account?>();
				captainList.Add(captain);
				AccountDto captainDto = _dtoTransformer.TurnAccountsIntoAccountDtos(captainList)[0];

				List<Alliance> allianceList = new List<Alliance>();
				allianceList.Add(this);
				AllianceDto allianceDto = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList)[0];

				_allianceRepository.AccountLeavesAlliance(captainDto, allianceDto);
			}

			foreach (var crewMember in _crewMembers)
			{
				List<Account?> crewMemberList = new List<Account?>();
				crewMemberList.Add(crewMember);
				AccountDto crewMemberDto = _dtoTransformer.TurnAccountsIntoAccountDtos(crewMemberList)[0];

				List<Alliance> allianceList = new List<Alliance>();
				allianceList.Add(this);
				AllianceDto allianceDto = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList)[0];

				_allianceRepository.AccountLeavesAlliance(crewMemberDto, allianceDto);
			}

			_allianceRepository.DeleteById(this._id);
		}

		public bool IsAccountInAlliance(Account? account)
		{
			List<Account?> accountList = new List<Account?>();
			accountList.Add(account);
			AccountDto accountDto = _dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

			List<Alliance> allianceList = new List<Alliance>();
			allianceList.Add(this);
			AllianceDto allianceDto = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList)[0];

			return _allianceRepository.IsAccountInAlliance(accountDto, allianceDto);
		}

		public void AccountJoinsAlliance(Account? account, bool isCaptain, ICommunityCollectionRepository communityCollectionRepository, ICommunityRepository communityRepository)
		{
			if (!IsAccountInAlliance(account) && IsAccountInCommunityThatAllianceIsIn(account, communityCollectionRepository, communityRepository))
			{
				List<Account?> accountList = new List<Account?>();
				accountList.Add(account);
				AccountDto accountDto = this._dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

				List<Alliance> allianceList = new List<Alliance>();
				allianceList.Add(this);
				AllianceDto allianceDto = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList)[0];

				_allianceRepository.AccountJoinsAlliance(accountDto, allianceDto, isCaptain);
			}
		}

		public void AccountLeavesAlliance(Account? account)
		{
			if (IsAccountInAlliance(account))
			{
				List<Account?> accountList = new List<Account?>();
				accountList.Add(account);
				AccountDto accountDto = this._dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];

				List<Alliance> allianceList = new List<Alliance>();
				allianceList.Add(this);
				AllianceDto allianceDto = _dtoTransformer.TurnAlliancesIntoAllianceDtos(allianceList)[0];

				_allianceRepository.AccountLeavesAlliance(accountDto, allianceDto);
			}
		}
	}
}
