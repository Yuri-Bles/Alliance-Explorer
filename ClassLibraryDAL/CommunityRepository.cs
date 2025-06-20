using System.Data;
using L4DTOs;
using L6InterfacesDAL;
using Microsoft.Data.SqlClient;

namespace L5DAL
{
	public class CommunityRepository : ICommunityRepository
	{
		private readonly DatabaseConnectionString _database = new DatabaseConnectionString();

		public AllianceDto GetAllianceById(int id)
		{
			AllianceDto allianceDto = new AllianceDto();

			string query1 =
				@"SELECT 
				ISNULL(ID, 0) AS ID,
				ISNULL(name, '') AS name,
				ISNULL(minimumAge, 0) AS minimumAge,
				ISNULL(maximumAge, 200) AS maximumAge,
				ISNULL(language, '') AS language,
				ISNULL(latitude, 0) AS latitude,
				ISNULL(longitude, 0) AS longitude,
				ISNULL(rules, '') AS rules,
				ISNULL(ageIsForced, 0) AS ageIsForced,
				ISNULL(onLocation, 0) AS onLocation,
				ISNULL(online, 1) AS online,
				ISNULL(allowCrewmemberEvents, 0) AS allowCrewmemberEvents
				FROM dbo.alliance
				WHERE ID = @ID
				";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query1, connection);

				connection.Open();

				command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					allianceDto = new AllianceDto
					{
						Id = reader.GetInt32(reader.GetOrdinal("ID")),
						Name = reader.GetString(reader.GetOrdinal("name")),
						MinimumAge = reader.GetInt32(reader.GetOrdinal("minimumAge")),
						MaximumAge = reader.GetInt32(reader.GetOrdinal("maximumAge")),
						Language = reader.GetString(reader.GetOrdinal("language")),
						Latitude = reader.GetDouble(reader.GetOrdinal("latitude")),
						Longitude = reader.GetDouble(reader.GetOrdinal("longitude")),
						Rules = reader.GetString(reader.GetOrdinal("rules")),
						AgeIsForced = Convert.ToBoolean(reader["ageIsForced"]),
						IsOnLocation = Convert.ToBoolean(reader["onLocation"]),
						IsOnline = Convert.ToBoolean(reader["online"]),
						AllowCrewmemberEvents = Convert.ToBoolean(reader["allowCrewmemberEvents"]),
					};
				}

				reader.Close();
				connection.Close();
			}

			string query2 = @"SELECT COUNT(*) FROM dbo.account_alliance 
							WHERE alliance_id = @AllianceId";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query2, connection);
				command.Parameters.Add("@AllianceId", SqlDbType.Int);

				connection.Open();
				
				command.Parameters["@AllianceId"].Value = allianceDto.Id;
				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						allianceDto.MemberCount = Convert.ToInt32(reader[0]);
					}
				}
				
				connection.Close();
			}

			return allianceDto;
		}

		public List<AllianceDto> GetAllAlliancesByCommunityId(int communityId)
		{
			List<AllianceDto> allianceDtOs = new List<AllianceDto>();

			string query1 =
				@"SELECT 
				ISNULL(ID, 0) AS ID,
				ISNULL(name, '') AS name,
				ISNULL(minimumAge, 0) AS minimumAge,
				ISNULL(maximumAge, 200) AS maximumAge,
				ISNULL(language, '') AS language,
				ISNULL(latitude, 0) AS latitude,
				ISNULL(longitude, 0) AS longitude,
				ISNULL(rules, '') AS rules,
				ISNULL(ageIsForced, 0) AS ageIsForced,
				ISNULL(onLocation, 0) AS onLocation,
				ISNULL(online, 1) AS online,
				ISNULL(allowCrewmemberEvents, 0) AS allowCrewmemberEvents
				FROM dbo.alliance
				WHERE community_id = @CommunityID
				";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query1, connection);

				connection.Open();

				command.Parameters.Add("@CommunityID", SqlDbType.Int).Value = communityId;

				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					AllianceDto alliance = new AllianceDto
					{
						Id = reader.GetInt32(reader.GetOrdinal("ID")),
						Name = reader.GetString(reader.GetOrdinal("name")),
						MinimumAge = reader.GetInt32(reader.GetOrdinal("minimumAge")),
						MaximumAge = reader.GetInt32(reader.GetOrdinal("maximumAge")),
						Language = reader.GetString(reader.GetOrdinal("language")),
						Latitude = reader.GetDouble(reader.GetOrdinal("latitude")),
						Longitude = reader.GetDouble(reader.GetOrdinal("longitude")),
						Rules = reader.GetString(reader.GetOrdinal("rules")),
						AgeIsForced = Convert.ToBoolean(reader["ageIsForced"]),
						IsOnLocation = Convert.ToBoolean(reader["onLocation"]),
						IsOnline = Convert.ToBoolean(reader["online"]),
						AllowCrewmemberEvents = Convert.ToBoolean(reader["allowCrewmemberEvents"]),
					};
					allianceDtOs.Add(alliance);
				}

				reader.Close();
				connection.Close();
			}

			string query2 = @"SELECT COUNT(*) FROM dbo.account_alliance 
							WHERE alliance_id = @AllianceId";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query2, connection);
				command.Parameters.Add("@AllianceId", SqlDbType.Int);

				connection.Open();
				foreach (var allianceDto in allianceDtOs)
				{
					command.Parameters["@AllianceId"].Value = allianceDto.Id;

					using SqlDataReader reader = command.ExecuteReader();
					if (reader.Read())
					{
						allianceDto.MemberCount = Convert.ToInt32(reader[0]);
					}
				}
				connection.Close();
			}

			return allianceDtOs;
		}

		public List<AccountDto> GetAllAccountsByCommunityId(int communityId)
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();

			string query1 =
				@"SELECT 
				ISNULL(a.ID, 0) AS ID, 
				ISNULL(a.name, '') AS name, 
				ISNULL(a.password, '') AS password, 
				ISNULL(a.email, '') AS email, 
				ISNULL(a.birthday, CAST('0001-01-01' AS date)) AS birthday, 
				ISNULL(a.latitude, 0) AS latitude, 
				ISNULL(a.longitude, 0) AS longitude, 
				ISNULL(a.maxDistance, 0) AS maxDistance 
				FROM account a
				INNER JOIN dbo.account_community ac ON a.ID = ac.account_id
				WHERE community_id = @CommunityID
				";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			command.Parameters.Add("@CommunityID", SqlDbType.Int).Value = communityId;

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

		public List<AccountDto> GetAdminsById(int communityId)
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();

			string query1 =
				@"SELECT 
				ISNULL(a.ID, 0) AS ID, 
				ISNULL(a.name, '') AS name, 
				ISNULL(a.password, '') AS password, 
				ISNULL(a.email, '') AS email, 
				ISNULL(a.birthday, CAST('0001-01-01' AS date)) AS birthday, 
				ISNULL(a.latitude, 0) AS latitude, 
				ISNULL(a.longitude, 0) AS longitude, 
				ISNULL(a.maxDistance, 0) AS maxDistance 
				FROM account a
				INNER JOIN dbo.account_community ac ON a.ID = ac.account_id
				WHERE community_id = @CommunityID
				AND isAdmin = 1
				";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			command.Parameters.Add("@CommunityID", SqlDbType.Int).Value = communityId;

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

		public List<AccountDto> GetMembersById(int communityId)
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();

			string query1 =
				@"SELECT 
				ISNULL(a.ID, 0) AS ID, 
				ISNULL(a.name, '') AS name, 
				ISNULL(a.password, '') AS password, 
				ISNULL(a.email, '') AS email, 
				ISNULL(a.birthday, CAST('0001-01-01' AS date)) AS birthday, 
				ISNULL(a.latitude, 0) AS latitude, 
				ISNULL(a.longitude, 0) AS longitude, 
				ISNULL(a.maxDistance, 0) AS maxDistance 
				FROM account a
				INNER JOIN dbo.account_community ac ON a.ID = ac.account_id
				WHERE community_id = @CommunityID
				AND isAdmin = 0
				";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			command.Parameters.Add("@CommunityID", SqlDbType.Int).Value = communityId;

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

		public int GetHighestAllianceId()
		{
			string query = "SELECT MAX(ID) FROM dbo.alliance";
			object result;

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query, connection);
				connection.Open();

				result = command.ExecuteScalar()!;

				if (result != DBNull.Value)
				{
					int highestId = Convert.ToInt32(result);
				}

				connection.Close();
			}

			return Convert.ToInt32(result);
		}

		public void CreateAlliance(AllianceDto allianceDto, int communityId)
		{
			string query = "INSERT INTO dbo.alliance (ID, community_id, name, minimumAge, maximumAge, language, latitude, longitude, rules, ageIsForced, onLocation, online, allowCrewmemberEvents) " +
						   "VALUES (@ID, @community_id, @name, @minimumAge, @maximumAge, @language, @latitude, @longitude, @rules, @ageIsForced, @onLocation, @online, @allowCrewmemberEvents)";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@ID", allianceDto.Id);
			command.Parameters.AddWithValue("@community_id", communityId);
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

			connection.Open();

			command.ExecuteNonQuery();

			connection.Close();
		}

		public bool IsAccountInCommunity(AccountDto accountDto, CommunityDto communityDto)
		{
			string query = "SELECT COUNT(*) FROM dbo.account_community " +
				"WHERE account_id = @account_id AND community_id = @community_id";
			object result;
			bool answer = false;

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@account_id", accountDto.Id);
			command.Parameters.AddWithValue("@community_id", communityDto.Id);

			connection.Open();

			result = command.ExecuteScalar()!;

			if (result != DBNull.Value && Convert.ToInt32(result) == 1)
			{
				answer = true;
			}

			connection.Close();
			return answer;
		}

		public void AccountJoinsCommunity(AccountDto accountDto, CommunityDto communityDto, bool isAdmin)
		{
			string query = "INSERT INTO dbo.account_community (account_id, community_id, isAdmin) " +
						   "VALUES (@account_id, @community_id, @isAdmin)";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@account_id", accountDto.Id);
			command.Parameters.AddWithValue("@community_id", communityDto.Id);
			command.Parameters.AddWithValue("@isAdmin", isAdmin);

			connection.Open();

			command.ExecuteNonQuery();

			connection.Close();
		}

		public void AccountLeavesCommunity(AccountDto accountDto, CommunityDto communityDto)
		{
			string query = "DELETE FROM dbo.account_community " +
						   "WHERE account_id = @account_id AND community_id = @community_id";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@account_id", accountDto.Id);
			command.Parameters.AddWithValue("@community_id", communityDto.Id);
			connection.Open();

			command.ExecuteNonQuery();

			connection.Close();
		}
	}
}
