using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class DashboardController : Controller
	{
		
		public IActionResult Index(string inst)
		{
			DashBoardCon dbc = new DashBoardCon();
			int res = dbc.getDatabaseCount(inst);
			TempData["Databases"] = res;
			return View();
		}
	}
}
