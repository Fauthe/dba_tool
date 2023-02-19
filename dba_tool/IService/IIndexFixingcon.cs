namespace dba_tool.IService
{
	public interface IIndexFixingcon
	{
		public void FixIndex(string dbname, string indexname, string schemaname, string tablename);
	}
}
