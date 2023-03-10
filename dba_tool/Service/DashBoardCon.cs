using dba_tool.Controllers;
using dba_tool.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dba_tool.Service
{
	public class DashBoardCon
	{

		LoginController lc;
		SqlDataReader dr;


		public int getDatabaseCount(string instance)
		{
			string result = "";
			try
			{

				string sql = "Select count(database_id) from sys.databases where database_id > 4;";
				SqlCommand cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;
				cmd.Connection = DBconnection.DBConnect(instance);
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
