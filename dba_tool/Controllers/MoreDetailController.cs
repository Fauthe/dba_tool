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
	}
}
