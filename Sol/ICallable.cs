using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol
{
	
	public interface ICallable
	{
		object DynamicInvoke(params object[] args);
	}
}
