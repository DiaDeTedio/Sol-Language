using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class BindExpression : Executable, IValue
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
				throw new NotImplementedException ();
			}
		}

		public IValue reference;
		public IValue bind;

		public override object Execute (params object [] args)
		{
			reference.setValue = bind.value;
			return reference.value;
		}

		public BindExpression(IValue reference, IValue bind)
		{
			this.reference = reference;
			this.bind = bind;
		}
	}
}
