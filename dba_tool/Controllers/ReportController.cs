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
		List<CPUusage> cpu = new List<CPUusage>();
		CPUusage cpus = new CPUusage();

		List<TempDBUsage> tdb = new List<TempDBUsage>();
		TempDBUsage tdbs = new TempDBUsage();

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
			server.Add(new ServerOverall()
			{
				CPUUsages = cpu,
				TempDBUs = tdb
			});
			return View(server);
		}

		public TempDBUsage getTempDbReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getTempdbUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					tdb.Add(new TempDBUsage()
					{
						serverName = dr["server_name"].ToString(),
						dbName = dr["database_name"].ToString(),
						fileName = dr["file_logical_name"].ToString(),
						filePath = dr["file_physical_name"].ToString(),
						fileSize = Convert.ToInt32(dr["file_size_mb"]),
						availableSpace = Convert.ToInt32(dr["available_space_mb"]),
						percentFull = Convert.ToDouble(dr["percent_full"])
					});
				}
				return tdbs;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
		public CPUusage getCPUReport()
		{
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getCpuUsage");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					cpu.Add(new CPUusage()
					{
						eventTime = Convert.ToDateTime(dr["EventTime2"]),
						sqlprocessutilization = Convert.ToInt32(dr["SQLProcessUtilization"]),
						systemIdel = Convert.ToInt32(dr["SystemIdle"]),
						otherProcessUtilization = Convert.ToInt32(dr["OtherProcessUtilization"]),
						loadDate = Convert.ToDateTime(dr["load_date"])
					});
				}
				return cpus;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
