using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class IndexFixingController : Controller
	{
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
			FixIndex(name, index, schema, table);
			await Task.Delay(1000);
			return Redirect("~/MoreDetail/MoreIndex");
		}

		public void FixIndex(string dbname, string indexname, string schemaname, string tablename)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_reOrganizeIndex");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@schemaname", schemaname);
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Parameters.AddWithValue("@tablename", tablename);
				cmd.Parameters.AddWithValue("@indexname", indexname);
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
