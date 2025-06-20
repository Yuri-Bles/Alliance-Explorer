namespace L3LogicLayer
{
	public class Account
	{
		public int Id { get; private set; }
		public string? Name { get; private set; }
		public string? Password { get; private set; }
		public string? Email { get; private set; }
		public DateOnly Birthday { get; private set; }
		public int Age { get; private set; }
		public double Latitude { get; private set; }
		public double Longitude { get; private set; }
		public int MaxDistance { get; private set; }

		public Account(int id, string? name, string? password, string? email, DateOnly birthday, double latitude, double longitude, int maxDistance)
		{
			this.Id = id;
			this.Name = name;
			this.Password = password;
			this.Email = email;
			this.Birthday = birthday;
			this.MaxDistance = maxDistance;

			this.Latitude = latitude is >= -90 and <= 90 ? latitude : 0;
			this.Longitude = longitude is >= -180 and <= 180 ? longitude : 0;

			var today = DateOnly.FromDateTime(DateTime.Today);
			int age = today.Year - birthday.Year;
			if (birthday > today.AddYears(-age))
			{
				age--;
			}
			this.Age = age;
		}
	}
}