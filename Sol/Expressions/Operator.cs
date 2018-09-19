using System;

namespace Sol.Expressions
{
	public class Operator : Expression
	{
		public OperatorType type;

		public Operator(OperatorType type)
		{
			this.type = type;
		}
		public Operator(TokenType token)
		{
			switch(token)
			{
				case TokenType.Plus:type = OperatorType.Add;break;
				case TokenType.Minus:type = OperatorType.Subtract;break;
				case TokenType.Divide:type = OperatorType.Divide;break;
				case TokenType.Multiply:type = OperatorType.Multiply;break;
				case TokenType.OpenParenthesis:type = OperatorType.OpenParenthesis;break;
				case TokenType.CloseParenthesis:type = OperatorType.CloseParenthesis;break;
			}
		}
		public override string ToString ()
		{
			return $"Operator : {type}";
		}
	}
	public enum OperatorType
	{
		Add,
		Subtract,
		Divide,
		Multiply,
		OpenParenthesis,
		CloseParenthesis
	}
}
