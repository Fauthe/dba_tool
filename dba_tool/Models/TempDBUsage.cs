namespace dba_tool.Models
{
	public class TempDBUsage
	{
		public string serverName { get; set; }
		public string dbName { get; set; }
		public string fileName { get; set; }
		public string filePath { get; set; }
		public int fileSize { get; set; }
		public int availableSpace { get; set; }
		public double percentFull { get; set; }
	}
}
