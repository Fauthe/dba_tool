using dba_tool.IService;
using dba_tool.Models;
using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class MoreDetailController : Controller
	{
		IndexPhysicalStat stats = new IndexPhysicalStat();

		Top20Tables t20ts = new Top20Tables();

		SQLLoginErrors logins = new SQLLoginErrors();

		databaseFiles dfs = new databaseFiles();
		DataFile dataFile = new DataFile();

		AllDBcon allDBcon = new AllDBcon();
		MoreDetailcon moreDetailcon = new MoreDetailcon();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult MoreIndex()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewBag.SelectedDB = db_name;
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			moreDetailcon.GetIndexDetails(db_name);
			return View(moreDetailcon.stat);
		}

		public IActionResult MoreTable()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			moreDetailcon.getTop20TableByDiskUsage(db_name);
			return View(moreDetailcon.t20t);
		}

		public IActionResult LoginError()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			//moreDetailcon.getLoginErrorLog();
			return View(moreDetailcon.login);
		}

		public IActionResult MoreDatabaseFiles()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			moreDetailcon.GetMoreDatabaseFiles(db_name);
			return View(moreDetailcon.df);
		}

		public IActionResult MoreView()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			moreDetailcon.getAllViews(db_name);
			return View(moreDetailcon.view);
		}

		


	}
}
