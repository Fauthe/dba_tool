using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	
	public class DBlistController : Controller
	{
		DBconnection db = new DBconnection();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SelectDB()
		{
			db.sql = "Select * from sys.databases";
			db.OnGet();

			return View();
		}
	}
}
