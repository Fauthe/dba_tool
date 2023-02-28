using dba_tool.Controllers;
using dba_tool.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace dba_tool
{
	public class DBconnection
	{
		//public string sql;
		//public void OnGet()
		//{
		//	try
		//	{
		//	Home = DESKTOP-6759IBQ
		//	Office = c
		//		String connectionString = "Data Source=DESKTOP-FM5935J\\TESTNODE; Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

		//		using (SqlConnection connection = new SqlConnection(connectionString))
		//		{
		//			connection.Open();
		//			SqlCommand cmd = new SqlCommand(sql, connection);
		//		}
		//	}
		//	catch (Exception ex)
		//	{

		//	}
		//}
		public static LoginController lc;





		public static SqlConnection DBConnect(string instance)
		{
			var conn = new SqlConnection();
			//conn.ConnectionString = "Data Source="+instance+";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			conn.ConnectionString = instance;
			if (conn.State != ConnectionState.Open)
			{
				conn.Open();

			}
			return conn;

		}
		public static DataTable GetTableByQuery(string SqlQuery)
		{
			
			try
			{
				SqlCommand command = new SqlCommand();
				//command.Connection = DBConnect();
				foreach (var item in lc.cs)
				{
					command.Connection = DBconnection.DBConnect(item.instances);
				}
				command.CommandText = SqlQuery;
				command.CommandType = System.Data.CommandType.Text;
				SqlDataAdapter adapter = new SqlDataAdapter(command);
				DataTable dt = new DataTable();
				adapter.Fill(dt);
				return dt;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public static void ExecuteNonQuery(String SqlQuery)
		{
			try
			{
				SqlCommand command = new SqlCommand();
				//command.Connection = DBConnect();
				foreach (var item in lc.cs)
				{
					command.Connection = DBconnection.DBConnect(item.instances);
				}
				command.CommandText = SqlQuery;
				command.CommandType = CommandType.Text;
				command.ExecuteNonQuery();

			}
			catch (Exception)
			{
				throw;
			}
		}

		public static SqlDataReader ExecuteQuery(String SqlQuery)
		{
			try
			{
				SqlCommand command = new SqlCommand();
				//command.Connection = DBConnect();
				foreach (var item in lc.cs)
				{
					command.Connection = DBconnection.DBConnect(item.instances);
				}
				command.CommandText = SqlQuery;
				command.CommandType = CommandType.Text;
				return command.ExecuteReader();

			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
