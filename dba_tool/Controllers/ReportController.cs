using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class ReportController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
