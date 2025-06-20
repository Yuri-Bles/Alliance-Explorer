using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4DTOs;
using L6InterfacesDAL;

namespace L7TestDAL
{
	public class TestAccountCollectionRepository : IAccountCollectionRepository
	{
		public int GetHighestId()
		{
			return 0;
		}

		public AccountDto GetAccountById(int id)
		{
			return new AccountDto();
		}

		public AccountDto GetAccountByName(string? name)
		{
			return new AccountDto();
		}

		public bool DoesNameAlreadyExist(string? name)
		{
			return false;
		}

		public void CreateAccount(AccountDto account)
		{

		}

		public List<AccountDto> GetAllAccounts()
		{
			return new List<AccountDto>();
		}

		public void DeleteAccountByName(string name)
		{
			throw new NotImplementedException();
		}
	}
}
