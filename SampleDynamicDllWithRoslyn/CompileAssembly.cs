using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace SampleDynamicDllWithRoslyn {

	public class CompileAssembly {

		public static void Main(string[] args) {

			// source to be compiled
			string classSource = @"using System;
namespace DynamicDll {
	public class SomeClass {
		public void SomeMethod() {
			// some source
			int y = 0;
			y++;
			if (y > 0) {
				Console.WriteLine(y);
			}
		}
	}
}";

			CompilerParameters options = new CompilerParameters {
				GenerateExecutable = false,
				GenerateInMemory = true,
				IncludeDebugInformation = true,
				CompilerOptions = "/define:DEBUG"
			};

			CodeDomProvider provider = new CSharpCodeProvider();
			//FixCompilerPath(provider);

			// The following line throws Exception, because compiler is copied to D:\bin\roslyn
			//
			// Ausnahme ausgelöst: "System.IO.DirectoryNotFoundException" in Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll
			// Ein Ausnahmefehler des Typs "System.IO.DirectoryNotFoundException" ist in Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll aufgetreten.
			// Ein Teil des Pfades "D:\Projects\SampleDynamicDllWithRoslyn\SampleDynamicDllWithRoslyn\bin\Debug\bin\roslyn\csc.exe" konnte nicht gefunden werden.
			CompilerResults result = provider.CompileAssemblyFromSource(options, classSource);

			// Keep console open
			Console.ReadLine();
		}

		/// <summary>
		/// Fixes the compiler path to get it run.
		/// </summary>
		private static void FixCompilerPath(CodeDomProvider provider) {
			FieldInfo memberInfo = provider.GetType().GetField("_compilerSettings", BindingFlags.Instance | BindingFlags.NonPublic);
			if (memberInfo != null) {
				object settings = memberInfo.GetValue(provider);
				FieldInfo path = settings.GetType().GetField("_compilerFullPath", BindingFlags.Instance | BindingFlags.NonPublic);
				path?.SetValue(settings, @"D:\bin\roslyn\csc.exe");
			}
		}

	}
}
