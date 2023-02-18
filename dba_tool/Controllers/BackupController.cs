using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class BackupController : Controller
	{
		SqlDataReader dr;
		List<dbs> dbss = new List<dbs>();

		List<Backups> backup = new List<Backups>();
		Backups backups = new Backups();

		List<TempDiffTran> tempdt = new List<TempDiffTran>();
		TempDiffTran tempdts = new TempDiffTran();
		public IActionResult Index()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return View();
		}

		public IActionResult FullBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			FetchData();
			return View(dbss);
		}

		[HttpPost]
		public IActionResult FullBackup(dbs db)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return View();
		}
		public async Task<IActionResult> TakeFullBak(string selectedDB, string fullbackup)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			if(selectedDB == null || fullbackup== null)
			{
				TempData["param"] = "Provide All Details";
				return RedirectToAction("FullBackup");
			}
			else
			{
				TakeFullBackup(selectedDB, fullbackup);
				await Task.Delay(1000);
				return Redirect("~/Report/BackupDetails");
			}
			
		}
		public IActionResult DifferentialBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			FetchData();
			return View(dbss);

		}

		[HttpPost]
		public IActionResult DifferentialBackup(dbs db)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");

			return View();
		}

		public IActionResult CheckFullBackupForDiff(string selectedDB, string diffbackup)
		{

			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			SpecificDatabaseBackupDetail(selectedDB);
			foreach (var item in tempdt)
			{
				ViewBag.a = item.recovery_model_desc;
				ViewBag.b = item.physical_device_name;
			}
			if(selectedDB == null || diffbackup == null)
			{
				TempData["param"] = "Provide All Details";
				return RedirectToAction("DifferentialBackup");
			}
			else
			{
				if (ViewBag.a != "Simple" && ViewBag.b != "")
				{

					return Redirect("TakeDifferentialBak/?dbname=" + selectedDB + " &diffbak=" + diffbackup);
				}
				else
				{
					TempData["successDetail"] = "Either Recovery Model is Simple or Full Backup is missing!!!";
					return RedirectToAction("DifferentialBackup");
				}
			}
			
		}

		public async Task<IActionResult> TakeDifferentialBak(string dbname, string diffbak)
		{

			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			TakeDifferentialBackup(dbname, diffbak);

			await Task.Delay(1000);

			return Redirect("~/Report/BackupDetails");
		}



		public IActionResult TransactionalBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			FetchData();
			return View(dbss);
		}

		[HttpPost]
		public IActionResult TransactionalBackup(dbs datas)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			FetchData();
			return View(dbss);
		}

		public IActionResult CheckFullBackupForTran(string selectedDB, string tranbackup)
		{

			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			SpecificDatabaseBackupDetail(selectedDB);
			foreach (var item in tempdt)
			{
				ViewBag.a = item.recovery_model_desc;
				ViewBag.b = item.physical_device_name;
			}
			if (selectedDB == null || tranbackup == null)
			{
				TempData["param"] = "Provide All Details";
				return RedirectToAction("TransactionalBackup");
			}
			else
			{
				if (ViewBag.a != "Simple" && ViewBag.b != "")
				{

					return Redirect("TakeTransactionalBak/?dbname=" + selectedDB + " &tranbak=" + tranbackup);
				}
				else
				{

					TempData["successDet"] = "Either Recovery Model is Simple or Full Backup is missing!!!";
					return RedirectToAction("TransactionalBackup");
				}
			}
		}

		public async Task<IActionResult> TakeTransactionalBak(string dbname, string tranbak)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			TakeTransactionalBackup(dbname, tranbak);

			await Task.Delay(1000);

			return Redirect("~/Report/BackupDetails");
		}

		public void FetchData()
		{
			try
			{

				string sql = "Select name from sys.databases where database_id > 4;";
				dr = DBconnection.ExecuteQuery(sql);
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

		public void TakeFullBackup(string dbname, string backupname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getFullBackup");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@backupname", backupname);
				cmd.Connection = DBconnection.DBConnect();
				cmd.ExecuteNonQuery();

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
			catch(Exception ex)
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

		public TempDiffTran SpecificDatabaseBackupDetail(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("upd_specificDatabaseBackupInfo");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
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
	}
}
