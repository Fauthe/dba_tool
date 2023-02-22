using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
