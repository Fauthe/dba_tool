namespace dba_tool.Models
{
	public class logSpace
	{
		public int id { get; set; }
		public long total_size { get; set; }
		public long used_size { get; set; }

		public long used_percent { get; set; }
	}
}
