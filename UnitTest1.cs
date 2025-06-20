using Alliance_Explorer;


namespace TestProjectAllianceExplorer
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void GetAllAlliancesFromCommunity()
		{
			List<Alliance> alliances = new List<Alliance>();

			Alliance_Explorer.DAL.AllianceDAL allianceDAL = new AllianceDAL();
		 	alliances = allianceDAL.GetAllAlliancesFromCommunity(0);
		}
	}
}