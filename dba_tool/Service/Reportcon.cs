using dba_tool.Controllers;
using dba_tool.IService;
using dba_tool.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dba_tool.Service
{
	public class Reportcon : IReportcon
	{
		LoginController lc;
		SqlDataReader dr;
		public List<CPUusage> cpu = new List<CPUusage>();
		CPUusage cpus = new CPUusage();

		public List<TempDBUsage> tdb = new List<TempDBUsage>();
		TempDBUsage tdbs = new TempDBUsage();

		public List<ServerOverall> server = new List<ServerOverall>();
		ServerOverall servers = new ServerOverall();

		public List<Backups> backup = new List<Backups>();
		Backups backups = new Backups();

		public List<SnapshotDetails> snapshot = new List<SnapshotDetails>();
		SnapshotDetails snapshots = new SnapshotDetails();

		public List<OverallLog> overallLog= new List<OverallLog>();
		OverallLog overallLogs = new OverallLog();

		public List<OverallMemory> memory = new List<OverallMemory>();
		OverallMemory memorys = new OverallMemory();

		public List<SelectiveDatabaseBackupDets> selectiveDatabaseBackupDet = new List<SelectiveDatabaseBackupDets>();
		SelectiveDatabaseBackupDets selectiveDatabaseBackupDets = new SelectiveDatabaseBackupDets();
		public void createSnapshot(string dbname, string snapname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_createSnapshot");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@snapshot", snapname);
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

		public void drop_snapshot(string snap_name, string inst)
		{
			try
			{
				string sql = $"Use master; Drop Database {snap_name};";
				DBconnection.ExecuteNonQuery(sql, inst);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public Backups getBackupReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getBackupsDetails");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					backup.Add(new Backups()
					{
						dbname = dr["name"].ToString(),
						last_backup_time = dr["last_backup_time"].ToString(),
						recovery_model = dr["recovery_model_desc"].ToString(),
						state = dr["state_desc"].ToString(),
						backup_type = dr["backup_type"].ToString(),
						backup_file_location = dr["physical_device_name"].ToString()
					}); ;
				}
				return backups;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public CPUusage getCPUReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getCpuUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					cpu.Add(new CPUusage()
					{
						eventTime = Convert.ToDateTime(dr["EventTime2"]),
						sqlprocessutilization = Convert.ToInt32(dr["SQLProcessUtilization"]),
						systemIdel = Convert.ToInt32(dr["SystemIdle"]),
						otherProcessUtilization = Convert.ToInt32(dr["OtherProcessUtilization"]),
						loadDate = Convert.ToDateTime(dr["load_date"])
					});
				}
				return cpus;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public SnapshotDetails getSnapshotDetails()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getSnapshotsDetails");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					snapshot.Add(new SnapshotDetails()
					{
						snapname = dr["snapshot_name"].ToString(),
						dbname = dr["source_database"].ToString(),
						created = Convert.ToDateTime(dr["create_date"]),
						state = dr["state_desc"].ToString(),
						snapshot_isolation_status = dr["snapshot_isolation_state_desc"].ToString(),
						recovery_model = dr["recovery_model_desc"].ToString()


					});
				}
				return snapshots;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public TempDBUsage getTempDbReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTempdbUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
				foreach (var item in lc.cs)
				{
					cmd.Connection = DBconnection.DBConnect(item.instances);
				}
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					tdb.Add(new TempDBUsage()
					{
						serverName = dr["server_name"].ToString(),
						dbName = dr["database_name"].ToString(),
						fileName = dr["file_logical_name"].ToString(),
						filePath = dr["file_physical_name"].ToString(),
						fileSize = Convert.ToInt32(dr["file_size_mb"]),
						availableSpace = Convert.ToInt32(dr["available_space_mb"]),
						percentFull = Convert.ToDouble(dr["percent_full"])
					});
				}
				return tdbs;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public OverallLog getOverallLogReport(string inst)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getOverallLogUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
					cmd.Connection = DBconnection.DBConnect(inst);
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					overallLog.Add(new OverallLog()
					{
						name = dr["DBName"].ToString(),
						log_used = Convert.ToDouble(dr["LogSize"]),
						log_used_percent = Convert.ToDouble(dr["LogSpaceUsed_Percent"])
					});
				}
				return overallLogs;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public OverallMemory getOverallMemoryReport(string inst)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getOverallMemoryUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Connection = DBconnection.DBConnect();
					cmd.Connection = DBconnection.DBConnect(inst);
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					memory.Add(new OverallMemory()
					{
						parameter = dr["Parameter"].ToString(),
						value = dr["Value"].ToString()
					});
				}
				return memorys;
				

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
		
		public SelectiveDatabaseBackupDets getUserInputDatabaseBackupDetails(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getUserInputDatabaseBackupDetails");
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
					selectiveDatabaseBackupDet.Add(new SelectiveDatabaseBackupDets()
					{
						server = dr["Server"].ToString(),
						database = dr["database_name"].ToString(),
						backup_start = Convert.ToDateTime(dr["backup_start_date"]),
						backup_end = Convert.ToDateTime(dr["backup_finish_date"]),
						backup_type = dr["backup_type"].ToString(),
						backup_size = Convert.ToInt64(dr["backup_size"]) / 1048576,
						backup_location = dr["physical_device_name"].ToString()
					});
				}
				return selectiveDatabaseBackupDets;


			}
			catch (Exception ex)
			{
				throw ex;

			}

		}
	}
}
