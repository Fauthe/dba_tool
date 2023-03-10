using dba_tool.Models;
using dba_tool.Service;
using Microsoft.AspNetCore.Mvc;

namespace dba_tool.Controllers
{
	public class LoginController : Controller
	{
		DBconnection db;
		GetConnectionString gcs = new GetConnectionString();
		public List<conString> cs = new List<conString>();
		public conString css = new conString();

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

		public IActionResult VerifyUsername(string instance, string username, string password)
		{
			try
			{

			
			if (instance == null)
			{
				TempData["ErrorMsg"] = "Enter Server Name";
				return Redirect("~/Login/Index");
			}
			string conn = gcs.ConnectionString(instance);
			DBconnection.constrs = conn;
			//cs.Add(new conString
			//{
			//	instances = conn
			//});
			//TempData["instance"] = instance;
			if (username != null && password != null)
			{
				lgn.checkUsername(username, conn);
				foreach (var item in lgn.Credential)
				{
					
					if (item.Username != null)
					{
						TempData["username"] = username;
						TempData["password"] = password;
						TempData["instance"] = conn;
						//TempData["ErrorMsg"] = "User Does Exists";
						//return Redirect("~/Login/Index");
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
			catch(Exception ex)
			{
				TempData["ErrorMsg"] = "Server is Not Accessible!!!";
				return Redirect("~/Login/Index");
			}



		}

		public IActionResult VerifyLogin()
		{
			//string inst = TempData["instance"].ToString();
			string usrname = TempData["username"].ToString();
			string password = TempData["password"].ToString();
			string conStrst = TempData["instance"].ToString();
			lgn.verifyLogin(usrname, password, conStrst);
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
					return Redirect("~/Dashboard/Index/?inst="+conStrst);
					//return Redirect("~/Login/Index");
				}
			}
			return View();
		}
	}

}
