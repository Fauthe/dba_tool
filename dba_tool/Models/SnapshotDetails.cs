namespace dba_tool.Models
{
	public class SnapshotDetails
	{
		public string snapname { get; set; }
		public string dbname { get; set; }
		public DateTime? created { get; set; }
		public string state { get; set; }
		public string snapshot_isolation_status { get; set; }
		public string recovery_model { get; set; }
	}
}
