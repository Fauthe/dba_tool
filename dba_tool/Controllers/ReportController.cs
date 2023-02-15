using dba_tool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace dba_tool.Controllers
{
	public class ReportController : Controller
	{
		SqlDataReader dr;
		List<ServerOverall> server = new List<ServerOverall>();
		ServerOverall servers = new ServerOverall();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ServerOverall()
		{
			ViewData["selecteddb"] = HttpContext.Session.GetString("selecteddb");
			getCPUReport();
			getTempDbReport();
			return View(server);
		}

		public ServerOverall getTempDbReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTempdbUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					server.Add(new ServerOverall()
					{
						serverName = dr["servername"].ToString(),
						dbName = dr["databasename"].ToString(),
						fileName = dr["filename"].ToString(),
						filePath = dr["physicalName"].ToString(),
						fileSize = Convert.ToInt32(dr["filesizeMB"]),
						availableSpace = Convert.ToInt32(dr["availableSpaceMB"]),
						percentFull = Convert.ToDouble(dr["percentfull"])
					});
				}
				return servers;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
		public ServerOverall getCPUReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getCpuUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					server.Add(new ServerOverall()
					{
						eventTime = Convert.ToDateTime(dr["EventTime2"]),
						sqlprocessutilization = Convert.ToInt32(dr["SQLProcessUtilization"]),
						systemIdel = Convert.ToInt32(dr["SystemIdle"]),
						otherProcessUtilization = Convert.ToInt32(dr["OtherProcessUtilization"]),
						loadDate = Convert.ToDateTime(dr["load_date"])
					});
				}
				return servers;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
