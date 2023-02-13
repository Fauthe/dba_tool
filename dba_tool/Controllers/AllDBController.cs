using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Drawing;

namespace dba_tool.Controllers
{
	public class AllDBController : Controller
	{
		List<dbs> dbss = new List<dbs>();
		List<logSpace> ls = new List<logSpace>();
		List<databaseFiles> df= new List<databaseFiles>();
		SqlDataReader dr;
		DBconnection db;
		logSpace lsp = new logSpace();
		databaseFiles dfs = new databaseFiles();
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
			FetchLogUsage(selectedDB);
			foreach(var item in ls)
			{
				ViewBag.totalLogSpace = item.total_size/1024;
				ViewBag.usedLogSpace = item.used_size / 1024;
				ViewBag.usedLogPercent = item.used_percent;
			}

			FetchDBFileLocations(selectedDB);
			foreach (var item in df)
			{
					ViewBag.dataFileName = item.file_name[0];
					ViewBag.dataFilePath = item.file_path[0];
					ViewBag.logFileName = item.file_name;
					ViewBag.logFilePath = item.file_path;
			}



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

		public logSpace FetchLogUsage(string dbname)
		{
			
			
			try
			{
				string sql = "use " + dbname + "; select database_id, total_log_size_in_bytes, used_log_space_in_bytes, used_log_space_in_percent" +
					"   from sys.dm_db_log_space_usage";
				dr = DBconnection.ExecuteQuery(sql);
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
				string sql = "use " + dbname + "; select name, physical_name" +
					"   from sys.database_files";
				dr = DBconnection.ExecuteQuery(sql);
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
