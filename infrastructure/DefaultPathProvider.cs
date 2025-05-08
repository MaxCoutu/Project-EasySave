using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.Infrastructure
{
	public class DefaultPathProvider : IPathProvider
	{
		private static readonly string BaseDir =
			@"C:\EasySave";

		public string GetLogDir()
		{
			string dir = Path.Combine(BaseDir, "Logs");
			Directory.CreateDirectory(dir);
			return dir;
		}

		public string GetStatusDir()
		{
			string dir = Path.Combine(BaseDir, "Status");
			Directory.CreateDirectory(dir);
			return dir;
		}
	}
}