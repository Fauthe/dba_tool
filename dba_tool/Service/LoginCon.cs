using dba_tool.IService;
using dba_tool.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace dba_tool.Service
{
	public class LoginCon : ILoginCon
	{
		SqlDataReader dr;
		public List<LoginCredential> Credential= new List<LoginCredential>();
		public LoginCredential Credentials = new LoginCredential();
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
						Username = dr["name"].ToString()

					});
				}
				return Credentials;

			}
			catch (Exception ex)
			{
				throw ex;

			}

		}
	}
}
