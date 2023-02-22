using dba_tool.Models;
using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class BackupController : Controller
	{
		//SqlDataReader dr;
		List<dbs> dbss = new List<dbs>();

		Backups backups = new Backups();

		TempDiffTran tempdts = new TempDiffTran();

		AllDBcon allDBcon= new AllDBcon();
		Backupcon backupcon = new Backupcon();
		public IActionResult Index()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return View();
		}

		public IActionResult FullBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchData();
			return View(allDBcon.dbss);
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
				backupcon.TakeFullBackup(selectedDB, fullbackup);
				await Task.Delay(1000);
				return Redirect("~/Report/SelectiveDatabaseBackupDetails/?database="+selectedDB);
			}
			
		}
		public IActionResult DifferentialBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchData();
			return View(allDBcon.dbss);

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
			backupcon.SpecificDatabaseBackupDetail(selectedDB);
			foreach (var item in backupcon.tempdt)
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
			backupcon.TakeDifferentialBackup(dbname, diffbak);

			await Task.Delay(1000);

			return Redirect("~/Report/SelectiveDatabaseBackupDetails/?database=" + dbname);
		}



		public IActionResult TransactionalBackup()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchData();
			return View(allDBcon.dbss);
		}

		[HttpPost]
		public IActionResult TransactionalBackup(dbs datas)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchData();
			return View(allDBcon.dbss);
		}

		public IActionResult CheckFullBackupForTran(string selectedDB, string tranbackup)
		{

			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			backupcon.SpecificDatabaseBackupDetail(selectedDB);
			foreach (var item in backupcon.tempdt)
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
			backupcon.TakeTransactionalBackup(dbname, tranbak);

			await Task.Delay(1000);

			return Redirect("~/Report/SelectiveDatabaseBackupDetails/?database=" + dbname);
		}

		
	}
}
