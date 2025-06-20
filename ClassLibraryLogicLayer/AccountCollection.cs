using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using L3LogicLayer;
using L4DTOs;
using L6InterfacesDAL;

namespace L3LogicLayer
{
	public class AccountCollection
	{
		private readonly IAccountCollectionRepository _accountCollectionRepository;
		private readonly DtoTransformer _dtoTransformer = new DtoTransformer();

		public List<Account?> Accounts { get; private set; } = new List<Account?>();

		public AccountCollection(IAccountCollectionRepository accountCollectionRepository)
		{
			this._accountCollectionRepository = accountCollectionRepository;

			this.Accounts = _dtoTransformer.TurnAccountDtosIntoAccounts(this._accountCollectionRepository.GetAllAccounts());
		}

		public bool LogInCheck(string? name, string? unhashedPassword)
		{
			if (name == null || unhashedPassword == null)
			{
				return false;
			}

			string? hashedPassword = PasswordHasher(unhashedPassword);

			AccountDto accountDto = _accountCollectionRepository.GetAccountByName(name);
			List<AccountDto> accountDtoList = new List<AccountDto>();
			accountDtoList.Add(accountDto);
			Account? account = _dtoTransformer.TurnAccountDtosIntoAccounts(accountDtoList)[0];

			return (hashedPassword == account!.Password);
		}

		internal string? PasswordHasher(string? unhashedPassword)
		{
			using SHA256 sha256 = SHA256.Create();
			byte[] inputBytes = Encoding.UTF8.GetBytes(unhashedPassword);
			byte[] hashBytes = sha256.ComputeHash(inputBytes);
				
			StringBuilder stringBuilder = new StringBuilder();
			foreach (var b in hashBytes)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		public string SignInCheck(string? name, string? password, string? email, DateOnly birthday, double latitude,
			double longitude)
		{
			string? errorMessage = null;
			if (name == null || name.Length < 5)
			{
				errorMessage = "Name must be at least five characters long.";
			}
			else if (_accountCollectionRepository.DoesNameAlreadyExist(name))
			{
				errorMessage = "Name is already taken.";
			}
			else if (password == null || password.Length < 8)
			{
				errorMessage = "Password must be at least eight characters long.";
			}
			else if (email == null || !email.Contains("@"))
			{
				errorMessage = "E-mail must have an @.";
			}
			else if (birthday > DateOnly.FromDateTime(DateTime.Today))
			{
				errorMessage = "Your birthday can't be in the future";
			}
			else if (latitude < -90 || latitude > 90)
			{
				errorMessage = "Latitude must be between -90 and 90";
			}
			else if (longitude < -180 || longitude > 180)
			{
				errorMessage = "Longitude must be between -180 and 180";
			}

			return errorMessage;
		}

		public void CreateAccount(string? name, string? password, string? email, DateOnly birthday, double latitude,
			double longitude, int maxDistance)
		{
			string? errorMessage = SignInCheck(name, password, email, birthday, latitude, longitude);
			password = PasswordHasher(password);
			if (_accountCollectionRepository.GetHighestId() != 0 && errorMessage == null)
			{
				int id = _accountCollectionRepository.GetHighestId() + 1;
				Account? account = new Account(id, name, password, email, birthday, latitude, longitude, maxDistance);

				List<Account?> accountList = new List<Account?>();
				accountList.Add(account);

				this.Accounts.Add(account);

				AccountDto accountDto = _dtoTransformer.TurnAccountsIntoAccountDtos(accountList)[0];
				_accountCollectionRepository.CreateAccount(accountDto);
			}
		}

		public Account? GetAccountByName(string? name)
		{
			List<AccountDto> accountDtoList = new List<AccountDto>();
			accountDtoList.Add(_accountCollectionRepository.GetAccountByName(name));
			return _dtoTransformer.TurnAccountDtosIntoAccounts(accountDtoList)[0];
		}
	}
}
