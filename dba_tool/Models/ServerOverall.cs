namespace dba_tool.Models
{
	public class ServerOverall
	{
		public DateTime eventTime { get; set; }
		public int sqlprocessutilization { get; set; }
		public int systemIdel { get; set; }
		public int otherProcessUtilization { get; set; }
		public DateTime loadDate { get; set; }
		public string serverName { get; set; }
		public string dbName { get; set; }
		public string fileName { get; set; }
		public string filePath { get; set; }
		public int fileSize { get; set; }
		public int availableSpace { get; set; }
		public double percentFull { get; set; }

	}
}
