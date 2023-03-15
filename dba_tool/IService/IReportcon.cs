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
		public void drop_snapshot(string snap_name, string inst);
		public OverallLog getOverallLogReport(string inst);
		public OverallMemory getOverallMemoryReport(string inst);
		public SelectiveDatabaseBackupDets getUserInputDatabaseBackupDetails(string dbname);

	}
}
