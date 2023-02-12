using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace dba_tool.Controllers
{
	public class AllDBController : Controller
	{
		List<dbs> dbss = new List<dbs>();
		dbs dbsss = new dbs();
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

		[HttpPost]
		public IActionResult index(dbs databs)
		{


			//tempdata.add("cdb",strddlvalue);
			return RedirectToAction("Dashboard");
		}

		public IActionResult Dashboard(string selectedDB)
		{
			ViewBag.SelectedDB = selectedDB;
			ViewBag.tableCount = GetTableCount(selectedDB);
			ViewBag.viewCount = GetViewsCount(selectedDB);
			ViewBag.indexCount = GetIndexesCount(selectedDB);
			return View();
		}

		public IActionResult DiskUsage()
		{
			//ViewBag.Name = name;
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

		public int GetTableCount(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTableCount");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public int GetViewsCount(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getViewsCount");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public int GetIndexesCount(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getIndexesCount");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				result = cmd.ExecuteScalar().ToString();
				return int.Parse(result);

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
