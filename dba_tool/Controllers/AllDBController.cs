using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Web.Providers.Entities;
//using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class AllDBController : Controller
	{

		SqlDataReader dr;
		DBconnection db;

		List<dbs> dbss = new List<dbs>();

		List<logSpace> ls = new List<logSpace>();
		logSpace lsp = new logSpace();

		List<databaseFiles> df= new List<databaseFiles>();
		databaseFiles dfs = new databaseFiles();

		List<SnapshotDetails> snapshot = new List<SnapshotDetails>();
		SnapshotDetails snapshots = new SnapshotDetails();
		
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


			return RedirectToAction("Dashboard");
		}

		public IActionResult TempDashboard()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			ViewData["sessionDB"] = HttpContext.Session.GetString("selecteddb");
			ViewBag.tableCount = GetTableCount(ViewData["sessionDB"].ToString());
			ViewBag.viewCount = GetViewsCount(ViewData["sessionDB"].ToString());
			ViewBag.indexCount = GetIndexesCount(ViewData["sessionDB"].ToString());
			FetchLogUsage(ViewData["sessionDB"].ToString());
			foreach (var item in ls)
			{
				ViewBag.totalLogSpace = item.total_size / 1024;
				ViewBag.usedLogSpace = item.used_size / 1024;
				ViewBag.usedLogPercent = item.used_percent;
			}

			FetchDBFileLocations(ViewData["sessionDB"].ToString());
			return View(df);
		}

		public IActionResult Dashboard(string selectedDB)
		{
			HttpContext.Session.SetString("selecteddb", selectedDB);

			ViewBag.SelectedDB = selectedDB;
			if(selectedDB == "Select Database" || selectedDB==null)
			{
				TempData["params"] = "Please, Select a Database";
				return RedirectToAction("index");
			}
			else
			{
				ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
				//string database = ViewData["selecteddb"].ToString();
				ViewBag.tableCount = GetTableCount(selectedDB);
				ViewBag.viewCount = GetViewsCount(selectedDB);
				ViewBag.indexCount = GetIndexesCount(selectedDB);
				FetchLogUsage(selectedDB);
				foreach (var item in ls)
				{
					ViewBag.totalLogSpace = item.total_size / 1024;
					ViewBag.usedLogSpace = item.used_size / 1024;
					ViewBag.usedLogPercent = item.used_percent;
				}

				FetchDBFileLocations(selectedDB);

				return View(df);
			}
			
		}

		public IActionResult Snapshot()
		{
			ViewBag.SelectedDB = HttpContext.Session.GetString("selecteddb");
			return View();
		}

		[HttpPost]
		public IActionResult Snapshot(SnapshotDetails snapshots)
		{
			
			return Redirect("~/report/Snap");
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

		public logSpace FetchLogUsage(string dbname)
		{
			
			
			try
			{

				SqlCommand cmd = new SqlCommand("udp_FetchLogUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					ls.Add(new logSpace()
					{
						id = Convert.ToInt32(dr["database_id"]),
						total_size = Convert.ToInt64(dr["total_log_size_in_bytes"]),
						used_size = Convert.ToInt64(dr["used_log_space_in_bytes"]),
						used_percent = Convert.ToInt64(dr["used_log_space_in_percent"])

					});
				}
				return lsp;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public databaseFiles FetchDBFileLocations(string dbname)
		{


			try
			{
				SqlCommand cmd = new SqlCommand("udp_FetchDBFileLocations");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					df.Add(new databaseFiles()
					{
						
						file_name = new List<string>() { dr["name"].ToString() },
						file_path = new List<string>() { dr["physical_name"].ToString() }

					});
				}
				return dfs;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}


		
		
	}
}
