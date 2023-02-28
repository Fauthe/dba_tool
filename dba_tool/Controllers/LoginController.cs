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

		public IActionResult VerifyUsername(string username, string password)
		{
			if (username != null && password != null)
			{
				lgn.checkUsername(username);
				foreach (var item in lgn.Credential)
				{
					
					if (item.Username != null)
					{
						TempData["username"] = username;
						TempData["password"] = password;
						//TempData["ErrorMsg"] = "User Does Exists";
						return RedirectToAction("VerifyLogin");
					}
				}

			}
			
			if (username==null&&password==null)
			{
				TempData["ErrorMsg"] = "Username and Password Field is Empty!!!";
				return Redirect("~/Login/Index");
			}
			if(username == null || password == null)
			{
				TempData["ErrorMsg"] = "Username or Password Field is Empty!!!";
				return Redirect("~/Login/Index");
			}
			if (lgn.Credentials.Username == null)
			{
				TempData["ErrorMsg"] = "User Does Not Exists";
				return Redirect("~/Login/Index");
			}


			return View();



		}

		public IActionResult VerifyLogin()
		{
			string usrname = TempData["username"].ToString();
			string password = TempData["password"].ToString();
			lgn.verifyLogin(usrname, password);
			foreach(var item in lgn.Credential1)
			{
				if(item.result==0)
				{
					TempData["ErrorMsg"] = "Password doesn't Matched";
					return Redirect("~/Login/Index");
				}
				if(item.result==1)
				{
					//TempData["ErrorMsg"] = "Login Successful";
					return Redirect("~/AllDB/Index");
				}
			}
			return View();
		}
	}

}
