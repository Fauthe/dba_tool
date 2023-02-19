namespace dba_tool.Models
{
	public class SelectiveDatabaseBackupDets
	{
		public string server { get; set; }
		public string database { get; set; }
		public DateTime backup_start { get; set; }
		public DateTime backup_end { get; set;}
		public string backup_type { get; set;}
		public long backup_size { get; set;}
		public string backup_location { get; set; }
	}
}
