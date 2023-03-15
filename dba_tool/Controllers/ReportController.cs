using dba_tool.Models;
using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class ReportController : Controller
	{
		CPUusage cpus = new CPUusage();

		TempDBUsage tdbs = new TempDBUsage();

		ServerOverall servers = new ServerOverall();

		Backups backups = new Backups();

		SnapshotDetails snapshots = new SnapshotDetails();

		Reportcon reportcon = new Reportcon();

		AllDBcon allDBcon= new AllDBcon();

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ServerOverall()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");

			reportcon.getCPUReport();
			reportcon.getTempDbReport();
			//reportcon.getOverallLogReport();
			//reportcon.getOverallMemoryReport();
			reportcon.server.Add(new ServerOverall()
			{
				CPUUsages = reportcon.cpu,
				TempDBUs = reportcon.tdb,
				OverallLogs = reportcon.overallLog,
				OverallMemorys = reportcon.memory
			});
			return View(reportcon.server);
		}

		public IActionResult BackupDetails()
		{

			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			reportcon.getBackupReport();
			return View(reportcon.backup);
		}


		public IActionResult SnapshotDetails()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			reportcon.getSnapshotDetails();
			return View(reportcon.snapshot);
		}

		public async Task<IActionResult> Snap(string database, string snapshot)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			if (database == null || snapshot == null)
			{
				TempData["param"] = "Provide All Details";
				return Redirect("~/alldb/Snapshot");
			}
			else
			{
				reportcon.createSnapshot(database, snapshot);
				await Task.Delay(1000);
				return RedirectToAction("SnapshotDetails");
			}
				
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

		public async Task<IActionResult> Temp_SnapshotDetails(string database, string inst)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			await Task.Delay(1000);
			reportcon.drop_snapshot(database, inst);
			


			return RedirectToAction("SnapshotDetails");
		}

		public IActionResult SelectiveDatabaseBackup(string inst)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchData(inst);
			return View(allDBcon.dbss); 
		}

		[HttpPost]
		public async Task<IActionResult> SelectiveDatabaseBackup(SelectiveDatabaseBackupDets sdbd)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			await Task.Delay(500);
			return View();
		}
		public IActionResult SelectiveDatabaseBackupDetails(string database)
		{
			if (database == "Select Database" || database == null)
			{
				TempData["params"] = "Please, Select a Database";
				return Redirect("SelectiveDatabaseBackup");
			}
			else
			{
				ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
				reportcon.getUserInputDatabaseBackupDetails(database);
				return View(reportcon.selectiveDatabaseBackupDet);
			}
			
		}

	}
}
