using dba_tool.IService;
using dba_tool.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using System.Web.Providers.Entities;
using System.Xml.Linq;

namespace dba_tool.Service
{
	public class LoginCon : ILoginCon
	{
		SqlDataReader dr;
		public List<LoginCredential> Credential= new List<LoginCredential>();
		public LoginCredential Credentials = new LoginCredential();

		public List<LoginCredential> Credential1 = new List<LoginCredential>();
		public LoginCredential Credentials1 = new LoginCredential();
		public LoginCredential checkUsername(string user)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_checkUsername");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@username", user);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Credential.Add(new LoginCredential()
					{
						Username = dr["name"].ToString(),
						Password = dr["password_hash"].ToString()

					});
				}
				return Credentials;

			}
			catch (Exception ex)
			{
				throw ex;

			}

		}

		public LoginCredential verifyLogin(string username, string password)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_VerifyLogin");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@username", username);
				cmd.Parameters.AddWithValue("@password", password);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					Credential1.Add(new LoginCredential()
					{
						result = Convert.ToInt32(dr["result"])

					});
				}
				return Credentials1;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
