using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Web.Providers.Entities;
//using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using dba_tool.Service;
using dba_tool.IService;

namespace dba_tool.Controllers
{
	public class AllDBController : Controller
	{


		logSpace lsp = new logSpace();

		databaseFiles dfs = new databaseFiles();

		SnapshotDetails snapshots = new SnapshotDetails();

		AllDBcon allDBcon = new AllDBcon();

		private readonly ILogger<AllDBController> _logger;

		public AllDBController(ILogger<AllDBController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			allDBcon.FetchData();
			
			return View(allDBcon.dbss);

		}

		[HttpPost]
		public IActionResult Index(dbs databs)
		{


			return RedirectToAction("Dashboard");
		}

		public IActionResult TempDashboard()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			ViewData["sessionDB"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.tableCount = allDBcon.GetTableCount(ViewData["sessionDB"].ToString());
			ViewBag.viewCount = allDBcon.GetViewsCount(ViewData["sessionDB"].ToString());
			ViewBag.indexCount = allDBcon.GetIndexesCount(ViewData["sessionDB"].ToString());
			allDBcon.FetchLogUsage(ViewData["sessionDB"].ToString());
			foreach (var item in allDBcon.ls)
			{
				ViewBag.totalLogSpace = item.total_size ;
				ViewBag.usedLogSpace = item.used_size ;
				ViewBag.usedLogPercent = item.used_percent;
			}

			allDBcon.FetchDBFileLocations(ViewData["sessionDB"].ToString());
			return View(allDBcon.df);
		}

		public IActionResult Dashboard(string selectedDB)
		{
			HttpContext.Session.SetString("selecteddb", selectedDB);

			ViewBag.SelectedDB = selectedDB;
			if(selectedDB == "Select Database" || selectedDB==null)
			{
				TempData["params"] = "Please, Select a Database";
				return RedirectToAction("index");
			}
			else
			{
				ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
				//string database = ViewData["selecteddb"].ToString();
				ViewBag.tableCount = allDBcon.GetTableCount(selectedDB);
				ViewBag.viewCount = allDBcon.GetViewsCount(selectedDB);
				ViewBag.indexCount = allDBcon.GetIndexesCount(selectedDB);
				allDBcon.FetchLogUsage(selectedDB);
				foreach (var item in allDBcon.ls)
				{
					ViewBag.totalLogSpace = item.total_size ;
					ViewBag.usedLogSpace = item.used_size ;
					ViewBag.usedLogPercent = item.used_percent;
				}

				allDBcon.FetchDBFileLocations(selectedDB);

				return View(allDBcon.df);
			}
			
		}

		public IActionResult Snapshot()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchData();
			return View(allDBcon.dbss);
		}

		[HttpPost]
		public IActionResult Snapshot(SnapshotDetails snapshots)
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return Redirect("~/report/Snap");
		}

		public IActionResult DiskUsage()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			var db_name = HttpContext.Session.GetString("selecteddb");
			allDBcon.FetchLogUsage(db_name);
			allDBcon.GetDataFileUsage(db_name);
			allDBcon.dataAndLogs.Add(new DataAndLog()
			{
				dataFiles = allDBcon.dataFiles,
				logSpaces = allDBcon.ls
			});
			return View(allDBcon.dataAndLogs);
		}



	}
}
