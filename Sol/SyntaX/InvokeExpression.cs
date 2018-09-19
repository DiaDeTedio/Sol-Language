using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class InvokeExpression : Executable, IValue
	{
		public dynamic value
		{
			get
			{
				return Execute();
			}
		}
		dynamic IValue.setValue
		{
			set
			{
				throw new NotSupportedException();
			}
		}
		public IValue function;
		public List<IValue> args;

		public override object Execute (params object[] args)
		{
			int size = this.args.Count;
			object[] fArgs = new object[size];
			for(int i=0;i<size;i++)
				fArgs[i] = getArg(this.args[i]);
			return function.value.DynamicInvoke(fArgs);
		}
		object getArg(IValue arg)
		{
			return arg.value;
		}

		public InvokeExpression(Value function, List<IValue> args)
		{
			this.function = function;
			this.args = args;
		}
	}
}
