using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class FunctionExpression : Container, ICallable
	{
		//public IValue reference;
		public string name;
		public string[] args;
		//public bool[] isPointer;

		public object DynamicInvoke(params object[] args)
		{
			int count = args.Length;
			for(int i=0;i<count;i++)
			{
				string par = this.args[i];
				object arg = args[i];
				data.set(par, arg);
			}
			return ExecuteContainer();
		}

		public FunctionExpression(string name, params string[] args)
		{
			this.name = name;
			this.args = args;
			/*
			int l = args.Length;
			isPointer = new bool[l];
			for(int i=0;i<l;i++)
			{
				string arg = args[i];
				if(arg[0] == '$')
					isPointer[i] = true;
				else
					isPointer[i] = false;
				this.args[i] = arg.Replace("$", "");
			}
			*/
		}
	}
}
