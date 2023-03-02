using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
