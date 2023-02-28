namespace dba_tool
{
	public class GetConnectionString
	{
		public string ConnectionString(string connectionString)
		{
			string con = "Data Source=" + connectionString + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
			return con;
		}

	}
}
