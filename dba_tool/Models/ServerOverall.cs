namespace dba_tool.Models
{
	public class ServerOverall
	{
		public List<TempDBUsage> TempDBUs { get; set;}
		public List<CPUusage> CPUUsages { get; set;}
		public List<OverallLog> OverallLogs { get; set;}
		public List<OverallMemory> OverallMemorys { get; set; }
	}
}
