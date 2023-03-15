using dba_tool.IService;
using dba_tool.Models;
using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dba_tool.Controllers
{
	public class DashboardController : Controller
	{
		
		public IActionResult Index(string inst)
		{
			Reportcon rc = new Reportcon();
			DashBoardCon dbc = new DashBoardCon();
			MoreDetailcon md = new MoreDetailcon();
			int res = dbc.getDatabaseCount(inst);
			TempData["Databases"] = res;

			//double res1 = dbc.getOverallCpu(inst);
			//TempData["CPU"] = res1;

			rc.getOverallMemoryReport(inst);
			md.getLoginErrorLog(inst);
			rc.getOverallLogReport(inst);
			rc.server.Add(new ServerOverall
			{
				OverallMemorys = rc.memory,
				loginErrors = md.login,
				OverallLogs = rc.overallLog
			});
			return View(rc.server);
		}
	}
}
