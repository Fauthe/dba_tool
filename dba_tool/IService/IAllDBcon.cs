using dba_tool.Models;

namespace dba_tool.IService
{
	public interface IAllDBcon
	{
		public void FetchData(string inst);
		public int GetTableCount(string dbname);
		public int GetViewsCount(string dbname);
		public int GetIndexesCount(string dbname);
		public logSpace FetchLogUsage(string dbname);
		public databaseFiles FetchDBFileLocations(string dbname);
		public DataFile GetDataFileUsage(string dbname);


	}
}
