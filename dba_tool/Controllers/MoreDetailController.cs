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
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult MoreIndex()
		{


			return View();
		}

		public string GetIndexDetails(string dbname)
		{
			string result = "";
			try
			{
				SqlCommand cmd = new SqlCommand("udp_getIndexPhysicalStat");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@dbname", dbname);
				cmd.Connection = DBconnection.DBConnect();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{

				}
					return result;

			}
			catch (Exception ex)
			{
				throw ex;

			}
		}
	}
}
