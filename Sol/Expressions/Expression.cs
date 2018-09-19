using System;
using System.Collections.Generic;
using DLR = System.Linq.Expressions.Expression;

namespace Sol.Expressions
{
	public class Expression
	{
		public FastExpressions childs;

		public virtual DLR ToDlr()
		{
			return DLR.Empty();
		}
	}
}
