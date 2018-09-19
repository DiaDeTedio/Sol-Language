using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class ConditionalExpression : Container, IExecutable, IValue
	{
		public dynamic value
		{
			get
			{
				return compare();
			}
		}

		public dynamic setValue
		{
			set
			{
				throw new NotImplementedException ();
			}
		}

		public IValue a;
		public IValue b;
		public TokenType comparisson;
		public ElseExpression Else;

		public object Execute (params object [] args)
		{
			bool result = compare();
			if(result)
			{
				ExecuteContainer();
			}else
			{
				Else?.DynamicInvoke();
			}
			return result;
		}
		public bool compare()
		{
			dynamic ax = a.value;
			dynamic bx = b.value;
			switch(comparisson)
			{
				case TokenType.Equal:return ax == bx;
				case TokenType.Different:return ax != bx;
				case TokenType.Larger:return ax > bx;
				case TokenType.Lower:return ax < bx;
				case TokenType.LargerOrEqual:return ax >= bx;
				case TokenType.LowerOrEqual:return ax <= bx;
			}
			return false;
		}

		public ConditionalExpression(IValue a, IValue b, TokenType comparisson)
		{
			this.a = a;
			this.b = b;
			this.comparisson = comparisson;
		}
	}
	public class ElseExpression : Container, ICallable
	{
		public object DynamicInvoke (params object [] args)
		{
			return ExecuteContainer();
		}
	}
}
