using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class MathExpression : Executable, IValue
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
		public override object Execute (params object[] args)
		{
			childs.Reset();
			return getValue();
		}
		dynamic getValue()
		{
			if(childs.size == 0)
				return null;
			var aToken = childs.Dequeue();
			var asig = aToken as Operator;
			var a = aToken as IValue;
			dynamic aval = a?.value;
			if(asig != null && asig.type == OperatorType.OpenParenthesis)
				aval = getValue();
			if(childs.count == 0)
				return aval;
			var op = childs.Dequeue() as Operator;
			if(op.type == OperatorType.CloseParenthesis)
				return aval;
			var b = getValue();
			switch(op.type)
			{
				case OperatorType.Add:return aval+b;
				case OperatorType.Subtract:return aval-b;
				case OperatorType.Divide:return aval/b;
				case OperatorType.Multiply:return aval*b;
			}
			return aval;
		}
		public override Dlr ToDlr ()
		{
			childs.Reset();
			Dlr x = getDlr();

			return x;
		}
		public Delegate Compile()
		{
			var dlr = ToDlr();
			return Dlr.Lambda(dlr).Compile();
		}
		Dlr getDlr()
		{
			var aToken = childs.Dequeue();
			var asig = aToken as Operator;
			if(asig != null && asig.type == OperatorType.OpenParenthesis)
				return getDlr();
			var a = aToken as Value;
			if(childs.count == 0)
				return a.value;
			var op = childs.Dequeue() as Operator;
			if(op.type == OperatorType.CloseParenthesis)
				return Dlr.Constant(a.value);
			var b = getValue();
			switch(op.type)
			{
				case OperatorType.Add:return Dlr.Add(Dlr.Constant(a.value),Dlr.Constant(b));
				case OperatorType.Subtract:return Dlr.Subtract(Dlr.Constant(a.value),Dlr.Constant(b));
				case OperatorType.Divide:return Dlr.Divide(Dlr.Constant(a.value),Dlr.Constant(b));
				case OperatorType.Multiply:return Dlr.Multiply(Dlr.Constant(a.value),Dlr.Constant(b));
			}
			return Dlr.Empty();
		}

		public MathExpression(List<Expression> expressions)
		{
			this.childs = new FastExpressions(expressions);
		}
	}
}
