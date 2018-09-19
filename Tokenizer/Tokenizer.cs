using System;
using System.Collections.Generic;
using S = Sol.Separator;

namespace Sol
{
	public class Tokenizer
	{
		public static bool isValue(TokenType token)
		{
			switch(token)
			{
				case TokenType.IntegerLiteral:return true;
				case TokenType.FloatLiteral:return true;
				case TokenType.TrueLiteral:return true;
				case TokenType.FalseLiteral:return true;
				case TokenType.StringLiteral:return true;
				case TokenType.ID:return true;
			}
			return false;
		}
		public static bool isComparisson(TokenType token)
		{
			switch(token)
			{
				case TokenType.Equal:return true;
				case TokenType.Different:return true;
				case TokenType.Larger:return true;
				case TokenType.Lower:return true;
				case TokenType.LargerOrEqual:return true;
				case TokenType.LowerOrEqual:return true;
			}
			return false;
		}
		public static bool isOperator(TokenType token)
		{
			switch(token)
			{
				case TokenType.Plus:return true;
				case TokenType.Minus:return true;
				case TokenType.Divide:return true;
				case TokenType.Multiply:return true;
				case TokenType.OpenParenthesis:return true;
				case TokenType.CloseParenthesis:return true;
			}
			return false;
		}
		public static List<char> begins = new List<char>()
		{
			//'"', '<', '[', '(', '{', '\''
			'"', '\''
		};
		public static List<char> ends = new List<char>()
		{
			//'"', '>', ']', ')', '}', '\''
			'"', '\''
		};
		public static List<char> special = new List<char>()
		{
			//'<','>','(',')','[',']','{','}'
		};
		public static List<bool> ables = new List<bool>()
		{
			true, true, true, true, true, true
		};
		public static List<bool> intern = new List<bool>()
		{
			true, true, true, true, true, true
		};

		public List<Token> Tokenize(string entry)
		{
			//entry = entry.Replace("\n","");
			entry = entry.Replace("\r","");
			var tokens = new List<Token>();
			var pieces = splitString(entry, new S(' ', false), new S('*'), new S('/'), new S('+'), new S('-'),
			                         new S('('), new S(')'), new S('\n'), new S(','), new S(';'),
			                         new S('='), new S('{'), new S('}'),
			                         new S('['), new S(']'), new S('&'), new S('$'));
			foreach(var piece in pieces)
			{
				var token = Token.build(piece);
				tokens.Add(token);
			}
			return tokens;
		}





		public static List<string> splitString(string text, params Separator[] separatorsArr)
		{
			List<Separator> separators = new List<Separator>(separatorsArr);
			List<string> list = new List<string>();
			string cur = "";
			Stack<int> opens = new Stack<int>();
			foreach(var c in text)
			{
				if(special.Contains(c))
				{
					list.Add(c.ToString());
					continue;
				}
				var separator = separators.Find(s => s.separator == c);
				if(separator != null && opens.Count == 0)
				{
					if(separator.include)
						if(!separator.addSep)
							cur += c;
					list.Add(cur);
					if(separator.include && separator.addSep)
						list.Add(c.ToString());
					cur = "";
					continue;
				}
				if(begins.Contains(c) && ((opens.Count > 0 && begins[opens.Peek()] != c) || opens.Count == 0))
				{
					opens.Push(begins.IndexOf(c));
					if(ables[opens.Peek()])
						cur += c;
					continue;
				}
				if(opens.Count == 0)
				{
					cur += c;
				}
				else
				{
					int pop = opens.Peek();
					char end = ends[pop];
					bool can = intern[pop];
					if(can && (c != end || ables[pop]))
						cur += c;
					if(c == end)
					{
						opens.Pop();
					}
				}
			}
			if(cur != "")
				list.Add(cur);
			list.RemoveAll((obj) => obj == "");
			applyList(list);
			return list;
		}
		static void applyList(List<string> list)
		{
			Queue<string> tokens = new Queue<string>(list);
			list.Clear();
			while(tokens.Count > 0)
			{
				string token = tokens.Dequeue();
				if(tokens.Count > 0)
				{
					string next = tokens.Peek();
					if(token == "=" && next == "=")
					{
						tokens.Dequeue();
						list.Add("==");
						continue;
					}
				}
				list.Add(token);
			}
		}
	}
	public class Separator
	{
		public char separator;
		public bool include;
		public bool addSep;

		public Separator(char separator, bool include = true, bool addSeparated = true)
		{
			this.separator = separator;
			this.include = include;
			this.addSep = addSeparated;
		}
	}
}
