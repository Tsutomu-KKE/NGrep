using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NGrep
{
	/// <summary>
	/// ex.) NGrep pattern *.cs
	/// </summary>
	class NGrep
	{
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				Console.Error.WriteLine("{0} pattern [filePattern=* [dir=. ]]", Path.GetFileName(asm.Location));
				return;
			}
			try
			{
				Grep(new Regex(args[0]), args.Length > 1 ? args[1] : "*", args.Length > 2 ? args[2] : ".");
			}
			catch (Exception ee)
			{
				Console.WriteLine(ee.Message);
				return;
			}
		}
		static void Grep(Regex r, string filePattern, string dir)
		{
			foreach (var fnam in Directory.GetFiles(dir, filePattern))
			{
				using (StreamReader sr = new StreamReader(fnam, Encoding.GetEncoding(932)))
				{
					Grep(r, fnam, sr);
				}
			}
			foreach (var subdir in Directory.GetDirectories(dir)) Grep(r, filePattern, subdir);
		}
		static void Grep(Regex r, string fnam, StreamReader sr)
		{
			int i = 0;
			string s;
			while ((s = sr.ReadLine()) != null)
			{
				++i;
				if (r.IsMatch(s))
					Console.WriteLine("{0} {1}: {2}", fnam, i, s.Substring(0, Math.Min(s.Length, 120)));
			}
		}
	}
}
