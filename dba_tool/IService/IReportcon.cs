using dba_tool.Models;

namespace dba_tool.IService
{
	public interface IReportcon
	{
		public TempDBUsage getTempDbReport();
		public CPUusage getCPUReport();
		public Backups getBackupReport();
		public void createSnapshot(string dbname, string snapname);
		public SnapshotDetails getSnapshotDetails();
		public void drop_snapshot(string snap_name);
		public OverallLog getOverallLogReport();
		public OverallMemory getOverallMemoryReport();
		public SelectiveDatabaseBackupDets getUserInputDatabaseBackupDetails(string dbname);

	}
}
