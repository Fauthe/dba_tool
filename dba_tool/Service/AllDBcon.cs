using dba_tool.IService;
using dba_tool.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using dba_tool.Controllers;

namespace dba_tool.Service
{
	public class AllDBcon : IAllDBcon
	{
		LoginController lc;
		SqlDataReader dr;
		public List<dbs> dbss = new List<dbs>();

		public List<logSpace> ls = new List<logSpace>();
		logSpace lsp = new logSpace();

		public List<databaseFiles> df = new List<databaseFiles>();
		databaseFiles dfs = new databaseFiles();

		public List<SnapshotDetails> snapshot = new List<SnapshotDetails>();
		SnapshotDetails snapshots = new SnapshotDetails();

		public List<DataFile> dataFiles = new List<DataFile>();
		DataFile dataFile = new DataFile();

		public List<DataAndLog> dataAndLogs = new List<DataAndLog>();
		DataAndLog dataAndLog = new DataAndLog();
		public void FetchData(string inst)
		{
			try
			{

				string sql = "Select name from sys.databases where database_id > 4;";
				dr = DBconnection.ExecuteQuery(sql, inst);
				while (dr.Read())
				{
					dbss.Add(new dbs()
					{
						Name = dr["name"].ToString()
					});
				}

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public databaseFiles FetchDBFileLocations(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_FetchDBFileLocations");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					df.Add(new databaseFiles()
					{

						file_name = new List<string>() { dr["name"].ToString() },
						file_path = new List<string>() { dr["physical_name"].ToString() }

					});
				}
				return dfs;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public logSpace FetchLogUsage(string dbname)
		{
			try
			{

				SqlCommand cmd = new SqlCommand("udp_FetchLogUsage");
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
					ls.Add(new logSpace()
					{
						id = Convert.ToInt32(dr["database_id"]),
						total_size = Convert.ToInt64(dr["total_log_size_in_bytes"])/1048576,
						used_size = Convert.ToInt64(dr["used_log_space_in_bytes"]) / 1048576,
						used_percent = Convert.ToInt64(dr["used_log_space_in_percent"])

					});
				}
				return lsp;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public int GetIndexesCount(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getIndexesCount");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public int GetTableCount(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTableCount");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public int GetViewsCount(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTableCount");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public DataFile GetDataFileUsage(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getDataFileUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@name", dbname);
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					dataFiles.Add(new DataFile()
					{
						name = dr["FileName"].ToString(),
						reserved = Convert.ToDouble(dr["CurrentSizeMB"]),
						used = Convert.ToDouble(dr["UsedSizeMB"]),
						unused = Convert.ToDouble(dr["FreeSpaceMB"])

					});
				}
				return dataFile;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}


	}
}
