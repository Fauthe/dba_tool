namespace dba_tool.Models
{
	public class databaseFiles
	{
		public List<string> file_name { get; set; }
		public List<string> file_path { get; set;}
		public string name { get; set; }
		public string path { get; set; }
		public string type { get; set; }
		public string state { get; set; }
		public double size { get; set; }
		public double max_size { get; set; }
		public int growth { get; set; }
	}
}
