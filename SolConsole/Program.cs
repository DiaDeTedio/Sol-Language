using System;
using Sol;
using Sol.SyntaX;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace SolConsole
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			var compiler = new Compiler();
			compiler.tokenizer = new Tokenizer();
			Data data = new Data();
			//compiler.data = data;
			data.set("print", (Action<object>)print);
			data.set("import", (Action<string>)import);
			//SolType.MapType(typeof(exampleClass), false, true);
			while(true)
			{
				Console.ResetColor();
				clear();
				string code = read(">");
				string fileName = "RUNTIME";
				bool debugMode = true;
				if(code.Contains("-debug"))
				{
					code = code.Replace("-debug", "");
					code = code.Trim();
					debugMode = false;
				}
				if(code.Contains(".txt") || code.Contains(".sol"))
				{
					fileName = Path.GetFileName(code);
					code = File.ReadAllText(code);
				}
				List<Token> tk = null;
				if(code.Contains(".sun"))
				{
					fileName = Path.GetFileName(code);
					tk = ByteCode.FromBytes(File.ReadAllBytes(code));
				}
				Script script = null;
				try
				{
					if(tk == null)
						script = compiler.Compile(code, data, fileName, new CompilerOptions{debugMode = debugMode});
					else
						script = compiler.CompileDirect(tk, data, fileName, new CompilerOptions{debugMode = debugMode});
				}catch(CompilationException compEx)
				{
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Compilation Error");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(compEx);
					pause();
					continue;
				}
				Stopwatch sw = new Stopwatch();
				sw.Start();
				try
				{
					for(int i=0;i<1;i++)
						script.Execute();
				}catch(Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Runtime Error");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(ex);
					Console.WriteLine($"In file -> {Sol.Debug.name} <-, at line {Sol.Debug.line+1}");
					pause();
					continue;
				}
				sw.Stop();
				print("Elapsed Time : "+sw.Elapsed.ToString());
				print("Elapsed Miliseconds : "+sw.ElapsedMilliseconds.ToString()+"ms");
				print(script.Execute());
				pause();
			}
		}
		static void import(string typeName)
		{
			Type type = Type.GetType(typeName);
			SolType.MapType(type, false, true);
		}
		static void clear()
		{
			Console.Clear();
		}
		static void print(object text)
		{
			Console.WriteLine(text);
		}
		static string read(string prefix)
		{
			Console.Write(prefix);
			return Console.ReadLine();
		}
		static void pause()
		{
			Console.ReadKey(true);
		}
	}
	public class exampleClass
	{
		public static string exampleField = "Hello World";

		public static void call(string arg)
		{
			
		}
	}
}
