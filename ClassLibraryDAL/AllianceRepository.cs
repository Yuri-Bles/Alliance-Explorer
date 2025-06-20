using L4DTOs;
using L6InterfacesDAL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace L5DAL
{
	public class AllianceRepository : IAllianceRepository
	{
		private readonly DatabaseConnectionString _database = new DatabaseConnectionString();

		public List<AccountDto> GetCaptainsByAllianceId(int allianceId)
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();

			string query1 =
				@"SELECT 
				ISNULL(ac.ID, 0) AS ID, 
				ISNULL(ac.name, '') AS name, 
				ISNULL(ac.password, '') AS password, 
				ISNULL(ac.email, '') AS email, 
				ISNULL(ac.birthday, CAST('0001-01-01' AS date)) AS birthday, 
				ISNULL(ac.latitude, 0) AS latitude, 
				ISNULL(ac.longitude, 0) AS longitude, 
				ISNULL(ac.maxDistance, 0) AS maxDistance 
				FROM account ac
				INNER JOIN dbo.account_alliance aa ON ac.ID = aa.account_id
				WHERE alliance_id = @AllianceID
				AND isCaptain = 1
				";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			command.Parameters.Add("@AllianceID", SqlDbType.Int).Value = allianceId;

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				AccountDto account = new AccountDto
				{
					Id = reader.GetInt32(reader.GetOrdinal("ID")),
					Name = reader.GetString(reader.GetOrdinal("name")),
					Password = reader.GetString(reader.GetOrdinal("password")),
					Birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birthday"))),
					Email = reader.GetString(reader.GetOrdinal("email")),
					Latitude = reader.GetDouble(reader.GetOrdinal("latitude")),
					Longitude = reader.GetDouble(reader.GetOrdinal("longitude")),
					MaxDistance = reader.GetInt32(reader.GetOrdinal("maxDistance"))
				};
				accountDtOs.Add(account);
			}

			reader.Close();
			connection.Close();

			return accountDtOs;
		}

		public List<AccountDto> GetCrewmembersByAllianceId(int allianceId)
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();

			string query1 =
				@"SELECT 
				ISNULL(ac.ID, 0) AS ID, 
				ISNULL(ac.name, '') AS name, 
				ISNULL(ac.password, '') AS password, 
				ISNULL(ac.email, '') AS email, 
				ISNULL(ac.birthday, CAST('0001-01-01' AS date)) AS birthday, 
				ISNULL(ac.latitude, 0) AS latitude, 
				ISNULL(ac.longitude, 0) AS longitude, 
				ISNULL(ac.maxDistance, 0) AS maxDistance 
				FROM account ac
				INNER JOIN dbo.account_alliance aa ON ac.ID = aa.account_id
				WHERE alliance_id = @AllianceID
				AND isCaptain = 0
				";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			command.Parameters.Add("@AllianceID", SqlDbType.Int).Value = allianceId;

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				AccountDto account = new AccountDto
				{
					Id = reader.GetInt32(reader.GetOrdinal("ID")),
					Name = reader.GetString(reader.GetOrdinal("name")),
					Password = reader.GetString(reader.GetOrdinal("password")),
					Birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birthday"))),
					Email = reader.GetString(reader.GetOrdinal("email")),
					Latitude = reader.GetDouble(reader.GetOrdinal("latitude")),
					Longitude = reader.GetDouble(reader.GetOrdinal("longitude")),
					MaxDistance = reader.GetInt32(reader.GetOrdinal("maxDistance"))
				};
				accountDtOs.Add(account);
			}

			reader.Close();
			connection.Close();

			return accountDtOs;
		}

		public void ChangeSettings(AllianceDto allianceDto, int communityId)
		{
			string query = "UPDATE table_name SET " +
			               "name = @name," +
			               "minimumAge = @minimumAge," +
			               "maximumAge = @maximumAge," +
			               "language = @language," +
			               "latitude = @latitude," +
			               "longitude = @longitude," +
			               "rules = @rules," +
			               "ageIsForced = @ageIsForced," +
			               "onLocation = @onLocation," +
			               "online = @online," +
			               "allowCrewmemberEvents = @allowCrewmemberEvents," +
			               "WHERE ID = @ID;";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@name", allianceDto.Name);
			command.Parameters.AddWithValue("@minimumAge", (object?)allianceDto.MinimumAge ?? DBNull.Value);
			command.Parameters.AddWithValue("@maximumAge", (object?)allianceDto.MaximumAge ?? DBNull.Value);
			command.Parameters.AddWithValue("@language", allianceDto.Language);
			command.Parameters.AddWithValue("@latitude", (object?)allianceDto.Latitude ?? DBNull.Value);
			command.Parameters.AddWithValue("@longitude", (object?)allianceDto.Longitude ?? DBNull.Value);
			command.Parameters.AddWithValue("@rules", (object?)allianceDto.Rules ?? DBNull.Value);
			command.Parameters.AddWithValue("@ageIsForced", allianceDto.AgeIsForced);
			command.Parameters.AddWithValue("@onLocation", allianceDto.IsOnLocation);
			command.Parameters.AddWithValue("@online", allianceDto.IsOnline);
			command.Parameters.AddWithValue("@allowCrewmemberEvents", allianceDto.AllowCrewmemberEvents);
			command.Parameters.AddWithValue("@ID", allianceDto.Id);

			connection.Open();

			command.ExecuteNonQuery();
		}

		public void DeleteById(int id)
		{
			string query = "DELETE FROM dbo.alliance WHERE ID = @ID";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.Add("@ID", SqlDbType.Int);

			connection.Open();

			command.Parameters["@ID"].Value = id;

			command.ExecuteNonQuery();

			connection.Close();
		}

		public bool IsAccountInAlliance(AccountDto accountDto, AllianceDto allianceDto)
		{
			string query = "SELECT COUNT(*) FROM dbo.account_alliance " +
			               "WHERE account_id = @account_id AND alliance_id = @alliance_id";
			object result;
			bool answer = false;

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@account_id", accountDto.Id);
			command.Parameters.AddWithValue("@alliance_id", allianceDto.Id);

			connection.Open();

			result = command.ExecuteScalar();

			if (result != DBNull.Value && Convert.ToInt32(result) == 1)
			{
				answer = true;
			}

			connection.Close();
			return answer;
		}

		public void AccountJoinsAlliance(AccountDto accountDto, AllianceDto allianceDto, bool isCaptain)
		{
			string query = "INSERT INTO dbo.account_alliance (account_id, alliance_id, isCaptain) " +
						   "VALUES (@account_id, @alliance_id, @isCaptain)";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@account_id", accountDto.Id);
			command.Parameters.AddWithValue("@alliance_id", allianceDto.Id);
			command.Parameters.AddWithValue("@isCaptain", isCaptain);

			connection.Open();

			command.ExecuteNonQuery();

			connection.Close();
		}

		public void AccountLeavesAlliance(AccountDto accountDto, AllianceDto allianceDto)
		{
			string query = "DELETE FROM dbo.account_alliance " +
			               "WHERE account_id = @account_id AND alliance_id = @alliance_id";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@account_id", accountDto.Id);
			command.Parameters.AddWithValue("@alliance_id", allianceDto.Id);
			connection.Open();

			command.ExecuteNonQuery();

			connection.Close();
		}

		public int GetCommunityIdByAlliance(AllianceDto allianceDto)
		{
			int communityId = -1;

			string query1 =
				@"SELECT community_id FROM dbo.alliance
					WHERE ID = @ID;";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			command.Parameters.Add("@ID", SqlDbType.Int).Value = allianceDto.Id;

			SqlDataReader reader = command.ExecuteReader();

			if (reader.Read())
			{
				communityId = reader.GetInt32(0);
			}

			reader.Close();
			connection.Close();
			return communityId;
		}
	}
}
