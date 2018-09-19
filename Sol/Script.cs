using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol
{
	public class Script : Container
	{
		public object Execute()
		{
			try
			{
				object last = null;
				foreach(var line in lines)
				{
					var ex = line as IExecutable;
					if(ex != null)
						last = ex.Execute();
				}
				return last;
			}
			finally
			{
				Console.WriteLine($"In script -> {Debug.name}, at {Debug.line}");
			}
		}
		public SolFunction GetFunction(string name)
		{
			return (args) => data.get(name).DynamicInvoke(args);
		}
		public Action MakeDirect()
		{
			List<Dlr> body = new List<Dlr>();
			foreach(var line in lines)
			{
				var ex = line as IExecutable;
				if(ex == null)
					continue;
				body.Add(Dlr.Call(Dlr.Constant(ex), ex.GetType().GetMethod("Execute"),Dlr.Constant(new object[0])));
			}
			return Dlr.Lambda<Action>(Dlr.Block(body.ToArray())).Compile();
		}
	}
	public delegate dynamic SolFunction(params dynamic[] args);
}
