namespace L4DTOs
{
	public class CommunityDto
	{
		public int Id;
		public string Subject;
		public string? Description;
		public int MemberCount;
		public string? Language;
		public string? Rules;
		public List<AllianceDto> Alliances;
	}
}
