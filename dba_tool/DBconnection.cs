using Microsoft.Data.SqlClient;

namespace dba_tool
{
	public class DBconnection
	{
		public string sql;
		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=DESKTOP-FM5935J\\TESTNODE; Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					SqlCommand cmd = new SqlCommand(sql, connection);
				}
			}
			catch (Exception ex)
			{

			}
		}
	}
}
