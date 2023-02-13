using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class ChartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
