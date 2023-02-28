using dba_tool.Controllers;
using dba_tool.IService;
using dba_tool.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dba_tool.Service
{
	public class Backupcon : IBackupcon
	{
		LoginController lc;
		SqlDataReader dr;
		public List<dbs> dbss = new List<dbs>();

		public List<Backups> backup = new List<Backups>();
		Backups backups = new Backups();

		public List<TempDiffTran> tempdt = new List<TempDiffTran>();
		TempDiffTran tempdts = new TempDiffTran();
		public TempDiffTran SpecificDatabaseBackupDetail(string dbname)
		{

			
			try
			{
				SqlCommand cmd = new SqlCommand("upd_specificDatabaseBackupInfo");
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
					tempdt.Add(new TempDiffTran()
					{
						recovery_model_desc = dr["recovery_model_desc"].ToString(),
						physical_device_name = dr["physical_device_name"].ToString()
					}); ;
				}
				return tempdts;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Backups TakeDifferentialBackup(string dbname, string backupname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("upd_getDifferentialBackup");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@backupname", backupname);
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

		public void TakeFullBackup(string dbname, string backupname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getFullBackup");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@backupname", backupname);
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

		public Backups TakeTransactionalBackup(string dbname, string backupname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("upd_getTransactionalBackup");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@backupname", backupname);
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
	}
}
