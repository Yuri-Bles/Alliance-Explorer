using Microsoft.Data.SqlClient;
using L4DTOs;
using System.Data;
using L6InterfacesDAL;

namespace L5DAL
{
	public class CommunityCollectionRepository : ICommunityCollectionRepository
	{
		private readonly DatabaseConnectionString _database = new DatabaseConnectionString();

		private readonly List<string> _stringColumns = new List<string> { "subject", "language", "description", "rules" };

		public List<CommunityDto> GetAllCommunities()
		{
			List<CommunityDto> communityDtOs = new List<CommunityDto>();
			string query1 = 
				@"SELECT 
				ISNULL(ID, 0) AS ID,
				ISNULL(subject, '') AS subject,
				ISNULL(language, '') AS language,
				ISNULL(rules, '') AS rules,
				ISNULL(description, '') AS description
				FROM dbo.community";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query1, connection);

				connection.Open();

				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					CommunityDto community = new CommunityDto
					{
						Id = reader.GetInt32(reader.GetOrdinal("ID")),
						Subject = reader.GetString(reader.GetOrdinal("subject")),
						Language = reader.GetString(reader.GetOrdinal("language")),
						Rules = reader.GetString(reader.GetOrdinal("rules")),
						Description = reader.GetString(reader.GetOrdinal("description"))
					};
					communityDtOs.Add(community);
				}

				reader.Close();
				connection.Close();
			}

			string query2 = @"SELECT COUNT(*) FROM dbo.account_community 
							WHERE community_id = @CommunityId";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query2, connection);
				command.Parameters.Add("@CommunityId", SqlDbType.Int);

				connection.Open();
				foreach (var communityDto in communityDtOs)
				{
					command.Parameters["@CommunityId"].Value = communityDto.Id;

					using SqlDataReader reader = command.ExecuteReader();
					if (reader.Read())
					{
						communityDto.MemberCount = Convert.ToInt32(reader[0]);
					}
				}
				connection.Close();
			}

			return communityDtOs;
		}

		public CommunityDto GetCommunityById(int id)
		{
			CommunityDto community = new CommunityDto();
			string query = @"SELECT * FROM dbo.community WHERE ID = @Id";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			connection.Open();

			command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				community = new CommunityDto
				{
					Id = reader.GetInt32(reader.GetOrdinal("ID")),
					Subject = reader.GetString(reader.GetOrdinal("subject")),
					Language = reader.GetString(reader.GetOrdinal("language")),
					Rules = reader.IsDBNull(reader.GetOrdinal("rules")) ? null : reader.GetString(reader.GetOrdinal("rules")),
					Description = reader.GetString(reader.GetOrdinal("description"))
				};
			}

			reader.Close();
			connection.Close();

			return community;
		}

		public int GetMemberCount(CommunityDto communityDto)
		{
			string query = @"SELECT COUNT(*) FROM dbo.account_community 
							WHERE community_id = @CommunityId";

			using (SqlConnection connection = new SqlConnection(_database.Connection))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.Add("@CommunityId", SqlDbType.Int);

				connection.Open();
				
				command.Parameters["@CommunityId"].Value = communityDto.Id;

				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						communityDto.MemberCount = Convert.ToInt32(reader[0]);
					}
				}
				
				connection.Close();
			}

			return communityDto.MemberCount;
		}

		public int GetHighestId()
		{
			string query = "SELECT MAX(ID) FROM dbo.community";
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

		public void CreateCommunity(CommunityDto communityDto)
		{
			string query = "INSERT INTO dbo.community (ID, subject, language, description) VALUES (@ID, @subject, @language, @description)";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@ID", communityDto.Id);
			command.Parameters.AddWithValue("@subject", communityDto.Subject);
			command.Parameters.AddWithValue("@language", communityDto.Language);
			command.Parameters.AddWithValue("@description", communityDto.Description);

			connection.Open();

			command.ExecuteNonQuery();
		}

		public void DeleteCommunityById(int communityId)
		{
			string query = "DELETE FROM dbo.community WHERE ID = @CommunityID";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.Add("@CommunityId", SqlDbType.Int);

			connection.Open();

			command.Parameters["@CommunityId"].Value = communityId;

			command.ExecuteNonQuery();

			connection.Close();
		}

		public void UpdateString(int communityId, string updateValue, string newValue)
		{
			bool updateValueisValidStringColumn = false;


			//Prevents SQL Injection by seeing if updateValue is a column in the database
			foreach (var stringColumn in _stringColumns)
			{
				if (stringColumn == updateValue)
				{
					updateValueisValidStringColumn = true;
					break;
				}
			}

			if (updateValueisValidStringColumn)
			{
				string query = $"UPDATE dbo.community SET {updateValue} = @newValue WHERE ID = @CommunityID";

				using SqlConnection connection = new SqlConnection(_database.Connection);
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.Add("@CommunityId", SqlDbType.Int);
				command.Parameters.Add("@newValue", SqlDbType.Text);

				connection.Open();

				command.Parameters["@CommunityId"].Value = communityId;
				command.Parameters["@newValue"].Value = newValue;

				command.ExecuteNonQuery();

				connection.Close();
			}
		}
	}
}
