// C# Project by BaussHacker
using System;
using System.Windows.Forms;

namespace jRAT
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new MainForm());

            if (args.Length == 11)
            {
                string input = args[0];
                string filename = args[1];
                string outputfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\" + new Random().Next() + ".jar";
                string iconfile = args[2];
                string title = args[3];
                string description = args[4];
                string company = args[5];
                string product = args[6];
                string copyright = args[7];
                string trademark = args[8];
                string version = args[9];
                string assemblyversion = args[10];

                MessageBox.Show("Compiled file " + args[1] + " from " + args[0], "jRAT", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CompileCode.GetJarCode(input);
                CompileCode.AddArguments("-jar");
                CompileCode.Compile(filename, outputfile, iconfile, title, description, company, product, copyright, trademark, version, assemblyversion);

                MessageBox.Show("Compiled file " + args[1] + " from " + args[0], "jRAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid argument length: " + args.Length, "jRAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}
		
	}
}
