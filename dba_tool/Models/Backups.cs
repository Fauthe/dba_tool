namespace dba_tool.Models
{
	public class Backups
	{

		public string dbname { get; set; }
		public string last_backup_time { get; set; }
		public string recovery_model { get; set; }
		public string state { get; set; }
		public string backup_type { get; set; }
		public string backup_file_location { get; set; }

	}
}
