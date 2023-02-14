namespace dba_tool.Models
{
	public class IndexPhysicalStat
	{
		public string schema_name { get; set; }
		public string object_name { get; set; }
		public string index_name { get; set; }
		public double avg_fragmentation_percent { get; set; }

	}
}
