using dba_tool.Models;
using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class LoginController : Controller
	{
		LoginCon lgn = new LoginCon();
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(LoginCredential lc)
		{
			return View(lgn.Credential);
		}

		public IActionResult VerifyLogin(string username, string password)
		{
			if(username==null&&password==null)
			{
				TempData["ErrorMsg"] = "Username and Password Field is Empty!!!";
				return Redirect("~/Login/Index");
			}
			if(username == null || password == null)
			{
				TempData["ErrorMsg"] = "Username or Password Field is Empty!!!";
				return Redirect("~/Login/Index");
			}
			lgn.checkUsername(username);
			if (username != null && password != null)
			{
				foreach (var item in lgn.Credential)
				{
					if (username != null & item.Username == null)
					{
						TempData["ErrorMsg"] = "User Does Not Exists";
						return Redirect("~/Login/Index");
					}
					if (item.Username != null)
					{
						TempData["ErrorMsg"] = "User Does Exists";
						return Redirect("~/Login/Index");
					}
				}
				
			}
			return View();



		}
	}

}
