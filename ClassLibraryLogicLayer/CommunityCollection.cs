using L4DTOs;
using L3LogicLayer;
using L5DAL;
using L6InterfacesDAL;

namespace L3LogicLayer
{
	public class CommunityCollection
	{
		public List<Community?> Communities { get; private set; } = new List<Community?>();
		private readonly ICommunityCollectionRepository _communityCollectionRepository;

		private readonly List<string> _nonNullableStringColumns = new List<string> { "subject", "language" };

		public CommunityCollection(ICommunityCollectionRepository communityCollectionRepository, ICommunityRepository communityRepository)
		{
			this._communityCollectionRepository = communityCollectionRepository;

			List<CommunityDto> communitiesDto = communityCollectionRepository.GetAllCommunities();
			List<Community> communities = new List<Community>();
			foreach (var communityDto in communitiesDto)
			{
				Community? community = new Community(communityRepository, communityDto.Subject, communityDto.Language!, communityDto.Description!, communityDto.Id, communityDto.MemberCount, communityDto.Rules!);
				this.Communities.Add(community);
			}
		}

		public void CreateCommunity(string subject, string language, string description, Account? account, ICommunityRepository communityRepository)
		{
			Community? community = new Community(communityRepository, subject, language, description);
			community.SetId(this._communityCollectionRepository.GetHighestId() + 1);
			Communities.Add(community);

			CommunityDto communityDto = new CommunityDto();
			communityDto.Id = _communityCollectionRepository.GetHighestId() + 1;
			communityDto.Subject = subject;
			communityDto.Language = language;
			communityDto.Description = description;

			_communityCollectionRepository.CreateCommunity(communityDto);

			community.AccountJoinsCommunity(account, true);
		}

		public Community? FindCommunityById(int id, ICommunityRepository communityRepository)
		{
			CommunityDto communityDto = _communityCollectionRepository.GetCommunityById(id);
			communityDto.MemberCount = _communityCollectionRepository.GetMemberCount(communityDto);

			Community? community = new Community(communityRepository, communityDto.Subject, communityDto.Language!, communityDto.Description!, communityDto.Id, communityDto.MemberCount, communityDto.Rules!);

			return community;
		}

		public List<Community?> GetAllCommunities()
		{
			return this.Communities;
		}

		public void DeleteCommunityById(Community? community)
		{
			foreach (var admin in community!.GetAdmins())
			{
				community.AccountLeavesCommunity(admin);
			}

			foreach (var member in community.GetMembers())
			{
				community.AccountLeavesCommunity(member);
			}

			Communities.Remove(community);
			_communityCollectionRepository.DeleteCommunityById(community.Id);
		}

		public void UpdateString(Community? community, string updateValue, string newValue)
		{
			bool incorrectValue = false;

			foreach (var nonNullableStringColumn in _nonNullableStringColumns)
			{
				if (updateValue == nonNullableStringColumn && newValue == null)
				{
					incorrectValue = true;
				}
			}

			if (!incorrectValue)
			{
				if (newValue == null)
				{
					newValue = string.Empty;
				}

				_communityCollectionRepository.UpdateString(community!.Id, updateValue, newValue);
			}
		}

		public string CreateCheck(string subject, string language, string description)
		{
			string errorMessage = null;
			if (subject.Length < 1)
			{
				errorMessage = "Subject cannot be empty";
			}
			else if (language.Length < 1)
			{
				errorMessage = "Language cannot be empty";
			}
			else if (description.Length < 1)
			{
				errorMessage = "Description cannot be empty";
			}
			return errorMessage!;
		}
	}
}
