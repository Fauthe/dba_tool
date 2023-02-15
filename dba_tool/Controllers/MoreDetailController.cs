using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class MoreDetailController : Controller
	{
		SqlDataReader dr;
		List<IndexPhysicalStat> stat = new List<IndexPhysicalStat>();
		IndexPhysicalStat stats = new IndexPhysicalStat();

		List<Top20Tables> t20t= new List<Top20Tables>();
		Top20Tables t20ts = new Top20Tables();

		List<SQLLoginErrors> login=new List<SQLLoginErrors>();
		SQLLoginErrors logins = new SQLLoginErrors();

		List<databaseFiles> df = new List<databaseFiles>();
		databaseFiles dfs = new databaseFiles();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult MoreIndex()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			GetIndexDetails(db_name);
			return View(stat);
		}

		public IActionResult MoreTable()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			getTop20TableByDiskUsage(db_name);
			return View(t20t);
		}

		public IActionResult LoginError()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			getLoginErrorLog();
			return View(login);
		}

		public IActionResult MoreDatabaseFiles()
		{
			var db_name = HttpContext.Session.GetString("selecteddb");
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			GetMoreDatabaseFiles(db_name);
			return View(df);
		}

		public IndexPhysicalStat GetIndexDetails(string dbname)
		{
			//string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getIndexPhysicalStat");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					stat.Add(new IndexPhysicalStat()
					{
						schema_name = dr["Schema_Name"].ToString(),
						object_name = dr["Object_Name"].ToString(),
						index_name = dr["Index_Name"].ToString(),
						avg_fragmentation_percent = Convert.ToDouble(dr["Avg_Fragmentation_Percent"])
					});
				}
					return stats;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public Top20Tables getTop20TableByDiskUsage(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTop20TableByDiskUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					t20t.Add(new Top20Tables()
					{
						schemaName = dr["SchemaName"].ToString(),
						tableName = dr["TableName"].ToString(),
						records = Convert.ToInt64(dr["Records"]),
						reserved = Convert.ToDouble(dr["reserved_mb"]),
						data = Convert.ToDouble(dr["data_mb"]),
						index_size = Convert.ToDouble(dr["index_size_mb"]),
						unused = Convert.ToDouble(dr["unused_mb"])
					});
				}
				return t20ts;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public SQLLoginErrors getLoginErrorLog()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getLoginErrorLog");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					login.Add(new SQLLoginErrors()
					{
						date = Convert.ToDateTime(dr["LogDate"]),
						processInfo = dr["ProcessInfo"].ToString(),
						details = dr["Text"].ToString()
					});
				}
				return logins;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}

		public databaseFiles GetMoreDatabaseFiles(string dbname)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getDatabaseFiles");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					df.Add(new databaseFiles()
					{
						name = dr["name"].ToString(),
						path = dr["physical_name"].ToString(),
						type = dr["type_desc"].ToString(),
						state = dr["state_desc"].ToString(),
						size = Convert.ToDouble(dr["size"]),
						max_size = Convert.ToDouble(dr["max_size"]),
						growth = Convert.ToInt32(dr["growth"])

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
