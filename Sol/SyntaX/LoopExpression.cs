using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class LoopExpression : Container, IExecutable
	{
		public string iteratorName;
		public IValue begin;
		public IValue end;
		public IValue increment;
		public Reference iterator;
		public object Execute (params object [] args)
		{
			iterator.setValue = begin;
			dynamic iter = iterator.value;
			for(iter = begin.value;iter <= end.value;iter += increment.value)
			{
				iterator.setValue = iter;
				ExecuteContainer();
			}
			return null;
		}

		public LoopExpression(string iteratorName, IValue begin, IValue end, IValue increment)
		{
			this.iteratorName = iteratorName;
			this.begin = begin;
			this.end = end;
			this.increment = increment;
			iterator = new Reference(iteratorName, data);
		}
	}
}
