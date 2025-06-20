using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L4DTOs;

namespace L6InterfacesDAL
{
	public interface IAccountCollectionRepository
	{
		AccountDto GetAccountByName(string? name);
		bool DoesNameAlreadyExist(string? name);
		int GetHighestId();
		void CreateAccount(AccountDto accountDto);
		List<AccountDto> GetAllAccounts();
		void DeleteAccountByName(string name);
	}
}
