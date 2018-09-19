using System;

namespace Sol
{
	public class Token
	{
		//public static string functionText = "func";
		//public static string trueText = "true";
		//public static string falseText = "false";
		//public static string ifText = "if";
		//public static string elseText = "else";
		//public static string thenText = "then";
		//public static string endText = "end";
		public string text;
		public TokenType type;
		public dynamic value;

		public static Token build(string text)
		{
			object value = null;
			var type = TokenType.None;
			//text = text.ToLower();
			switch(text)
			{
				case "debug":type = TokenType.Debug;break;
				case "line":type = TokenType.Line;break;
				case "(":type = TokenType.OpenParenthesis;break;
				case ")":type = TokenType.CloseParenthesis;break;
				case "[":type = TokenType.OpenSquares;break;
				case "]":type = TokenType.CloseSquares;break;
				case "{":type = TokenType.OpenBrackets;break;
				case "}":type = TokenType.CloseBrackets;break;
				case "true":
				 type = TokenType.TrueLiteral;
				 value = true;
				 break;
				case "false":
				 type = TokenType.FalseLiteral;
				 value = false;
				 break;
				case "null":type = TokenType.NullLiteral;break;
				case "+":type = TokenType.Plus;break;
				case "-":type = TokenType.Minus;break;
				case "/":type = TokenType.Divide;break;
				case "*":type = TokenType.Multiply;break;
				case "if":type = TokenType.If;break;
				case "else":type = TokenType.Plus;break;
				case "function":type = TokenType.Function;break;
				case "global":type = TokenType.Field;break;
				case "public":type = TokenType.Public;break;
				case "private":type = TokenType.Private;break;
				case "static":type = TokenType.Static;break;
				case "internal":type = TokenType.Internal;break;
				case "export":type = TokenType.Export;break;
				case "type":type = TokenType.Type;break;
				case "for":type = TokenType.For;break;
				case "while":type = TokenType.While;break;
				case "foreach":type = TokenType.ForEach;break;
				case "void":type = TokenType.VoidLiteral;break;
				case "var":type = TokenType.Variable;break;
				case "=":type = TokenType.Bind;break;
				case "==":type = TokenType.Equal;break;
				case "!=":type = TokenType.Different;break;
				case ">":type = TokenType.Larger;break;
				case "<":type = TokenType.Lower;break;
				case ">=":type = TokenType.LargerOrEqual;break;
				case "<=":type = TokenType.LowerOrEqual;break;
				case "\n":type = TokenType.EOF;break;
				case ";":type = TokenType.EOF;break;
				case ",":type = TokenType.Comma;break;
				case "&":type = TokenType.Referencer;break;
				case "$":type = TokenType.UnReferencer;break;

				case "return":type = TokenType.Return;break;
				case "then":type = TokenType.Then;break;
				case "end":type = TokenType.End;break;
			}
			if(type == TokenType.None)
			{
				if(text[0] == '"' && text[text.Length-1] == '"')
				{
					type = TokenType.StringLiteral;
					value = text.Remove(0, 1).Remove(text.Length-2, 1);
				}else
				{
					if(tryID(text))
					{
						type = TokenType.ID;
						value = text;
						goto end;
					}
					int integer;
					if(int.TryParse(text, out integer))
					{
						type = TokenType.IntegerLiteral;
						value = integer;
						goto end;
					}
					float single;
					if(float.TryParse(text.Replace('.', ','), out single))
					{
						type = TokenType.FloatLiteral;
						value = single;
						goto end;
					}
					char c;
					if(char.TryParse(text, out c))
					{
						type = TokenType.CharLiteral;
						value = c;
					}
				}
			}
			end:
			return new Token(text, type, value);
		}
		static bool tryID(string text)
		{
			if(text == "")
				return false;
			if(char.IsDigit(text[0]))
				return false;
			if(text[0] == '.')
				return false;
			foreach(var c in text)
			{
				if(char.IsDigit(c) || char.IsLetter(c) || c == '_' || c == '.' || c == '$')
					continue;

				return false;
			}
			return true;
		}

		public Token(string text, TokenType type, object value = null)
		{
			this.text = text;
			this.type = type;
			this.value = value;
		}
		public override string ToString ()
		{
			if(value != null)
				return $"Token:{text} : {type} = {value}";
			return $"Token:{text} : {type}";
		}
	}
}
