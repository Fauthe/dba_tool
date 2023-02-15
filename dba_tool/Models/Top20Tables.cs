namespace dba_tool.Models
{
	public class Top20Tables
	{

		public string schemaName { get; set; }
		public string tableName { get; set; }
		public long records { get; set; }
		public double reserved { get; set; }
		public double data { get; set; }
		public double index_size { get; set; }
		public double unused { get; set; }
	}
}
