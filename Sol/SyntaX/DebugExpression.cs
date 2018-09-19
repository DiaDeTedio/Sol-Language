using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;

namespace Sol.SyntaX
{
	public class DebugExpression : Executable
	{
		public string name;
		public int line;
		public DebugSymbol symbol;
		public override object Execute (params object [] args)
		{
			switch(symbol)
			{
				case DebugSymbol.Name:
					Debug.name = name;
					return name;
				case DebugSymbol.Line:
					Debug.line = line;
					return line;
			}
			return null;
		}
		public DebugExpression(DebugSymbol symbol, string name, int line)
		{
			this.symbol = symbol;
			this.name = name;
			this.line = line;
		}
	}
	public enum DebugSymbol
	{
		Name,
		Line
	}
}
