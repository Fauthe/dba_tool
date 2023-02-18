using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			return View();
		}
	}
}
