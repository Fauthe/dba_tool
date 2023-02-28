using dba_tool.Controllers;
using dba_tool.IService;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dba_tool.Service
{
	public class IndexFixingcon : IIndexFixingcon
	{
		LoginController lc;
		public void FixIndex(string dbname, string indexname, string schemaname, string tablename)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_reOrganizeIndex");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@schemaname", schemaname);
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@tablename", tablename);
				cmd.Parameters.AddWithValue("@indexname", indexname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				cmd.ExecuteNonQuery();


			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
