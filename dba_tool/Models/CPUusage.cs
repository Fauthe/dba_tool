namespace dba_tool.Models
{
	public class CPUusage
	{
		
		public DateTime eventTime { get; set; }
		public int sqlprocessutilization { get; set; }
		public int systemIdel { get; set; }
		public int otherProcessUtilization { get; set; }
		public DateTime loadDate { get; set; }
		

	}
}
