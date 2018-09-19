using System;
using Sol;
using System.Collections.Generic;
using Sol.Expressions;
using Sol.SyntaX;

namespace SolConsole
{
	public static class MathParser
	{
		public static dynamic Parse(string entry, Tokenizer tokenizer, Data data)
		{
			return GetExpression(entry, tokenizer, data).Execute();
		}
		public static MathExpression GetExpression(string entry, Tokenizer tokenizer, Data data)
		{
			var tokens = tokenizer.Tokenize(entry);
			List<Expression> exprs = new List<Expression>();
			foreach(var token in tokens)
			{
				if(Tokenizer.isValue(token.type))
				{
					if(token.type == TokenType.ID)
						exprs.Add(new Reference(token.value, data));
					else
						exprs.Add(new Constant(token.value));
				}else if(Tokenizer.isOperator(token.type))
				{
					exprs.Add(new Operator(token.type));
				}
			}
			MathExpression math = new MathExpression(exprs);
			return math;
		}
	}
}
