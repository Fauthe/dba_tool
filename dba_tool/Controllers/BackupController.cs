using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dba_tool.Controllers
{
	public class BackupController : Controller
	{
		SqlDataReader dr;
		List<dbs> dbss = new List<dbs>();
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
			return RedirectToAction("TakeFullBak");
		}
		public async Task<IActionResult> TakeFullBak(string selectedDB, string database)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			TakeFullBackup(selectedDB, database);
			await Task.Delay(1000);
			return View("https://localhost:7065/Report/BackupDetails");
		}
		public IActionResult DifferentialBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return View();

		}

		

		public IActionResult TransactionalBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return View();
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
	}
}
