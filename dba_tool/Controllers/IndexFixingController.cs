using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class IndexFixingController : Controller
	{
		IndexFixingcon indexFixingcon = new IndexFixingcon();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Rebuild(string dbname, string indexName, string schemaName, string tableName)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.dbname = dbname;
			ViewBag.indexName = indexName;
			ViewBag.schemaName = schemaName;
			ViewBag.tableName = tableName;
			return View();
		}

		[HttpPost]
		public IActionResult Rebuild()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			return View();
		}

		public IActionResult Reorganize(string dbname, string indexName, string schemaName, string tableName)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.dbname = dbname;
			ViewBag.indexName = indexName;
			ViewBag.schemaName = schemaName;
			ViewBag.tableName = tableName;
			return View();
		}

		[HttpPost]
		public IActionResult Reorganize()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			return View();
		}

		public async Task<IActionResult> IndexFix(string name, string index, string schema, string table)
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			indexFixingcon.FixIndex(name, index, schema, table);
			await Task.Delay(1000);
			return Redirect("~/MoreDetail/MoreIndex");
		}

		
	}
}
