using System.IO;
using System.Reflection;

namespace GeoGebetter
{
	public static class ResourceManager
	{
		public static string FileDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\data\\";
		
		public static string ReadAllText(string _path)
		{
			return File.ReadAllText(FileDirectory + _path);
		}

		public static Stream OpenRead(string _path)
		{
			return File.OpenRead(FileDirectory + _path);
		}
	}
}