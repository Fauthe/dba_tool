using dba_tool.Controllers;
using dba_tool.IService;
using dba_tool.Models;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Net;

namespace dba_tool.Service
{

	public class MoreDetailcon : IMoreDetailcon
	{
		LoginController lc;
		SqlDataReader dr;
		public List<IndexPhysicalStat> stat = new List<IndexPhysicalStat>();
		IndexPhysicalStat stats = new IndexPhysicalStat();

		public List<Top20Tables> t20t = new List<Top20Tables>();
		Top20Tables t20ts = new Top20Tables();

		public List<SQLLoginErrors> login = new List<SQLLoginErrors>();
		SQLLoginErrors logins = new SQLLoginErrors();

		public List<databaseFiles> df = new List<databaseFiles>();
		databaseFiles dfs = new databaseFiles();

		public List<Views> view = new List<Views>();
		Views views= new Views();

		

		public IndexPhysicalStat GetIndexDetails(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getIndexPhysicalStat");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					stat.Add(new IndexPhysicalStat()
					{
						schema_name = dr["Schema_Name"].ToString(),
						object_name = dr["Object_Name"].ToString(),
						index_name = dr["Index_Name"].ToString(),
						avg_fragmentation_percent = Convert.ToDouble(dr["Avg_Fragmentation_Percent"])
					});
				}
				return stats;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public SQLLoginErrors getLoginErrorLog(string inst)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getLoginErrorLog");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
					cmd.Connection = DBconnection.DBConnect(inst);
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					login.Add(new SQLLoginErrors()
					{
						date = Convert.ToDateTime(dr["LogDate"]),
						processInfo = dr["ProcessInfo"].ToString(),
						details = dr["Text"].ToString()
					});
				}
				return logins;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public databaseFiles GetMoreDatabaseFiles(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getDatabaseFiles");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					df.Add(new databaseFiles()
					{
						name = dr["name"].ToString(),
						path = dr["physical_name"].ToString(),
						type = dr["type_desc"].ToString(),
						state = dr["state_desc"].ToString(),
						size = Convert.ToDouble(dr["size"]),
						max_size = Convert.ToDouble(dr["max_size"]),
						growth = Convert.ToInt32(dr["growth"])

					});
				}
				return dfs;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public Top20Tables getTop20TableByDiskUsage(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTop20TableByDiskUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					t20t.Add(new Top20Tables()
					{
						schemaName = dr["SchemaName"].ToString(),
						tableName = dr["TableName"].ToString(),
						records = Convert.ToInt64(dr["Records"]),
						reserved = Convert.ToDouble(dr["reserved_mb"]),
						data = Convert.ToDouble(dr["data_mb"]),
						index_size = Convert.ToDouble(dr["index_size_mb"]),
						unused = Convert.ToDouble(dr["unused_mb"])
					});
				}
				return t20ts;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public Views getAllViews(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getAllViews");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					view.Add(new Views()
					{
						view = dr["view_name"].ToString(),
						schema = dr["schema_name"].ToString(),
						createDate = Convert.ToDateTime(dr["create_date"]),
						updateDate= Convert.ToDateTime(dr["modify_date"])
					});
				}
				return views;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}


	}
}
