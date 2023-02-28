using dba_tool.Models;

namespace dba_tool.IService
{
	public interface ILoginCon
	{

		public LoginCredential checkUsername(string user);
		public LoginCredential verifyLogin(string username, string password);
	}
}
