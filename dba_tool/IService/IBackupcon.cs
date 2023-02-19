using dba_tool.Models;

namespace dba_tool.IService
{
	public interface IBackupcon
	{
		public void TakeFullBackup(string dbname, string backupname);
		public Backups TakeDifferentialBackup(string dbname, string backupname);
		public Backups TakeTransactionalBackup(string dbname, string backupname);
		public TempDiffTran SpecificDatabaseBackupDetail(string dbname);

	}
}
