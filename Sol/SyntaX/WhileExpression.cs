using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class WhileExpression : Container, IExecutable
	{
		public ConditionalExpression condition;

		public object Execute (params object [] args)
		{
			while(condition.compare())
				ExecuteContainer();
			return null;
		}

		public WhileExpression(ConditionalExpression condition)
		{
			this.condition = condition;
		}
	}
}
