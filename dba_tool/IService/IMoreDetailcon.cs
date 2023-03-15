using dba_tool.Models;

namespace dba_tool.IService
{
	public interface IMoreDetailcon
	{
		public IndexPhysicalStat GetIndexDetails(string dbname);
		public Top20Tables getTop20TableByDiskUsage(string dbname);
		public SQLLoginErrors getLoginErrorLog(string inst);
		public databaseFiles GetMoreDatabaseFiles(string dbname);
		public Views getAllViews(string dbname);



	}
}
