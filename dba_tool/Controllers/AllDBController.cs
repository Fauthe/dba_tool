using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace dba_tool.Controllers
{
	public class AllDBController : Controller
	{
		List<dbs> dbss = new List<dbs>();
		SqlCommand com = new SqlCommand();
		SqlDataReader dr;
		SqlConnection con = new SqlConnection();
		private readonly ILogger<AllDBController> _logger;

		public AllDBController(ILogger<AllDBController> logger)
		{
			_logger = logger;
			con.ConnectionString = "Data Source=DESKTOP-FM5935J\\TESTNODE; Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		}

		public IActionResult Index()
		{
			FetchData();
			return View(dbss);
		}

		public void FetchData()
		{
			try
			{
				con.Open();
				com.Connection = con;
				com.CommandText = "Select name from sys.databases where database_id > 4;";
				dr = com.ExecuteReader();
				while (dr.Read())
				{
					dbss.Add(new dbs()
					{
						Name = dr["name"].ToString()
					});
				}
				con.Close();
			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
