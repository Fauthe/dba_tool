using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class ReportController : Controller
	{
		SqlDataReader dr;
		List<CPUusage> cpu = new List<CPUusage>();
		CPUusage cpus = new CPUusage();

		List<TempDBUsage> tdb = new List<TempDBUsage>();
		TempDBUsage tdbs = new TempDBUsage();

		List<ServerOverall> server = new List<ServerOverall>();
		ServerOverall servers = new ServerOverall();

		List<Backups> backup = new List<Backups>();
		Backups backups = new Backups();

		List<SnapshotDetails> snapshot = new List<SnapshotDetails>();
		SnapshotDetails snapshots = new SnapshotDetails();

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ServerOverall()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			
			getCPUReport();
			getTempDbReport();
			server.Add(new ServerOverall()
			{
				CPUUsages = cpu,
				TempDBUs = tdb
			});
			return View(server);
		}

		public IActionResult BackupDetails()
		{

			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			getBackupReport();
			return View(backup);
		}


		public IActionResult SnapshotDetails()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			getSnapshotDetails();
			return View(snapshot);
		}

		public async Task<IActionResult> Snap(string database, string snapshot)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			createSnapshot(database, snapshot);
			await Task.Delay(1000);
			return RedirectToAction("SnapshotDetails");
		}

		public IActionResult DeleteSnap(string snapname)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.SelectedSnap = snapname;
			return View();
		}

		[HttpPost]
		public IActionResult DeleteSnap(SnapshotDetails snapshots)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			return RedirectToAction("Temp_SnapshotDetails");
		}

		public async Task<IActionResult> Temp_SnapshotDetails(string database)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			await Task.Delay(1000);
			drop_snapshot(database);
			


			return RedirectToAction("SnapshotDetails");
		}

		public TempDBUsage getTempDbReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTempdbUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
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
		public CPUusage getCPUReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getCpuUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
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
		public Backups getBackupReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getBackupsDetails");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
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

		public void createSnapshot(string dbname, string snapname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_createSnapshot");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@snapshot", snapname);
				cmd.Connection = DBconnection.DBConnect();
				cmd.ExecuteNonQuery();


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
				cmd.Connection = DBconnection.DBConnect();
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

		public void drop_snapshot(string snap_name)
		{
			try
			{
				string sql = $"Use master; Drop Database {snap_name};";
				DBconnection.ExecuteNonQuery(sql);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
