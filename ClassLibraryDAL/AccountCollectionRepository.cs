using System.Data;
using L4DTOs;
using L6InterfacesDAL;
using Microsoft.Data.SqlClient;

namespace L5DAL
{
	public class AccountCollectionRepository : IAccountCollectionRepository
	{
		private readonly DatabaseConnectionString _database = new DatabaseConnectionString();

		public AccountDto GetAccountByName(string? name)
		{
			AccountDto accountDto = new AccountDto();
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
				FROM dbo.account a
				WHERE CAST(name AS NVARCHAR(MAX)) = @name";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);
			command.Parameters.Add("@name", SqlDbType.NVarChar);

			connection.Open();

			command.Parameters["@name"].Value = name;

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				accountDto.Id = reader.GetInt32(reader.GetOrdinal("ID"));
				accountDto.Name = reader.GetString(reader.GetOrdinal("name"));
				accountDto.Password = reader.GetString(reader.GetOrdinal("password"));
				accountDto.Birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birthday")));
				accountDto.Email = reader.GetString(reader.GetOrdinal("email"));
				accountDto.Latitude = reader.GetDouble(reader.GetOrdinal("latitude"));
				accountDto.Longitude = reader.GetDouble(reader.GetOrdinal("longitude"));
				accountDto.MaxDistance = reader.GetInt32(reader.GetOrdinal("maxDistance"));
			}

			reader.Close();
			connection.Close();

			return accountDto;
		}

		public bool DoesNameAlreadyExist(string? name)
		{
			bool doesNameAlreadyExist = false;
			string query1 = @"SELECT COUNT(*)
							FROM dbo.account
							WHERE name = @name";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);
			command.Parameters.Add("@name", SqlDbType.NVarChar);

			connection.Open();

			command.Parameters["@name"].Value = name;
				
			int nameCount = (int)command.ExecuteScalar();
			if (nameCount > 0)
			{
				doesNameAlreadyExist = true;
			}

			connection.Close();

			return doesNameAlreadyExist;
		}

		public int GetHighestId()
		{
			int highestId = 0;

			string query = "SELECT MAX(ID) FROM dbo.account";
			object result;

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);
			connection.Open();

			result = command.ExecuteScalar()!;

			if (result != DBNull.Value)
			{
				highestId = Convert.ToInt32(result);
			}

			connection.Close();

			return highestId;
		}

		public void CreateAccount(AccountDto accountDto)
		{
			string query = "INSERT INTO dbo.account (ID, name, password, email, birthday, latitude, longitude, maxDistance) " +
						   "VALUES (@ID, @name, @password, @email, @birthday, @latitude, @longitude, @maxDistance)";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@ID", accountDto.Id);
			command.Parameters.AddWithValue("@name", accountDto.Name);
			command.Parameters.AddWithValue("@password", accountDto.Password);
			command.Parameters.AddWithValue("@email", accountDto.Email);
			command.Parameters.AddWithValue("@birthday", accountDto.Birthday);
			command.Parameters.AddWithValue("@latitude", accountDto.Latitude);
			command.Parameters.AddWithValue("@longitude", accountDto.Longitude);
			command.Parameters.AddWithValue("@maxDistance", accountDto.MaxDistance);

			connection.Open();

			command.ExecuteNonQuery();
		}

		public List<AccountDto> GetAllAccounts()
		{
			List<AccountDto> accountDtOs = new List<AccountDto>();
			string query1 =
				@"SELECT 
				ISNULL(ID, 0) AS ID,
				ISNULL(name, '') AS name,
				ISNULL(password, '') AS password,
				ISNULL(email, '') AS email,
				ISNULL(birthday, '') AS birthday,
				ISNULL(latitude, 0) AS latitude,
				ISNULL(longitude, 0) AS longitude,
				ISNULL(maxDistance, 0) AS maxDistance
				FROM dbo.account";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query1, connection);

			connection.Open();

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				AccountDto account = new AccountDto
				{
					Id = reader.GetInt32(reader.GetOrdinal("ID")),
					Name = reader.GetString(reader.GetOrdinal("name")),
					Password = reader.GetString(reader.GetOrdinal("password")),
					Email = reader.GetString(reader.GetOrdinal("email")),
					Birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birthday"))),
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

		public void DeleteAccountByName(string name)
		{
			string query = "DELETE FROM dbo.account WHERE name = @name";

			using SqlConnection connection = new SqlConnection(_database.Connection);
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.Add("@name", SqlDbType.NVarChar);

			connection.Open();

			command.Parameters["@name"].Value = name;

			command.ExecuteNonQuery();

			connection.Close();
		}
	}
}
