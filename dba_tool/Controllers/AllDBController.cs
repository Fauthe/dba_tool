using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace dba_tool.Controllers
{
	public class AllDBController : Controller
	{
		List<dbs> dbss = new List<dbs>();
		SqlDataReader dr;
		DBconnection db;
		private readonly ILogger<AllDBController> _logger;

		public AllDBController(ILogger<AllDBController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			FetchData();
			return View(dbss);
		}

		public IActionResult Dashboard()
		{
			return View();
		}

		public void FetchData()
		{
			try
			{
				string sql = "Select name from sys.databases where database_id > 4;";
				dr = DBconnection.ExecuteQuery(sql);
				while (dr.Read())
				{
					dbss.Add(new dbs()
					{
						Name = dr["name"].ToString()
					});
				}
				
			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
